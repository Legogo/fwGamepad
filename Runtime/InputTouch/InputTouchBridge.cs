using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace fwp.inputeer
{
    public class InputTouchBridge : MonoBehaviour
    {
        InputTouchPinch pinchBridge;
        InputTouchSwipe swipeBridge;
        HelperScreenTouchSequenceSolver sequencer;

        public bool useMainCamera = true;
        public bool useOverlayCast = false; // cast another ray to get everything in "overlay" canvas
        public Camera inputCamera;
        public LayerMask _layer;

        [Header("pinch|scroll")]
        public float mouseScrollMulFactor = 1f; // step / cran de souris
        public float touchSpreadMulFactor = 1f; // step / event de spread
        public Vector2 scrollClampMagnitude = Vector2.zero;
        public bool useOvershoot = false; // use progressive clamping
        public float overshootClampLength = 1f; // qty of overshoot possible
        public float overshootClampMulFactor = 1f; // speed move toward clamp

        [Header("swipe")]
        public float limitLifeTime = 0.5f;
        public float limitSwipeAmplitude = 200f;

        protected InputTouchFinger[] _fingers;
        InputTouchFinger _finger = null;
        Touch[] touches;

        public Action<InputTouchFinger> onTouch;
        public Action<InputTouchFinger> onRelease;
        public Action<InputTouchFinger> onOverring;
        public Action onTouching;

        private int touchCount = 0; // read only



        void Awake()
        {
            manager = this;

            DontDestroyOnLoad(gameObject);

            enabled = false;

            StartCoroutine(processSetup());

            pinchBridge = new InputTouchPinch(this);
            swipeBridge = new InputTouchSwipe(this);

            if (useDebugInBuild)
            {
                sequencer = new HelperScreenTouchSequenceSolver(new Rect[]{
          new Rect(0.9f, 0.9f, 0.1f, 0.1f),
          new Rect(0.9f, 0.9f, 0.1f, 0.1f)
        });

                sequencer.onToggle += delegate ()
                {
                    Debug.Log(getStamp() + "toggling drawDebug : " + drawDebug);
                    drawDebug = !drawDebug;
                };
            }

        }

        IEnumerator processSetup()
        {
            if (inputCamera == null)
            {
                while (inputCamera == null)
                {
                    fetchCamera();
                    yield return null;
                }
                Debug.Log(GetType() + " camera " + inputCamera.name + " is setup", inputCamera.gameObject);
            }

            int qtyFingers = 10;
            if (!isMobile()) qtyFingers = 2;

            //create all 11 touches
            _fingers = new InputTouchFinger[qtyFingers];
            for (int i = 0; i < _fingers.Length; i++)
            {
                _fingers[i] = new InputTouchFinger();
            }

#if UNITY_EDITOR
      if (drawDebug) Debug.LogWarning("debug drawing for " + GetType() + " is active");
#endif

            //Debug.Log(GetType() + " setup is done, enabling update");

            //EngineEventSystem.onPauseEvent += onSystemPause;
            //EngineEventSystem.onFocusEvent += onFocusPause;

            enabled = true;
        }

        public void subscribeToScroll(Action<float, float> onScroll)
        {
            pinchBridge.onScroll += onScroll;
        }

        public void subscribeToSwipe(Action<Vector2> onSwipeDelta)
        {
            swipeBridge.onSwipeDelta += onSwipeDelta;
        }

        protected void fetchCamera()
        {
            //debugOverlays = GameObject.FindObjectsOfType<DebugWindowSettings>();

            if (useMainCamera && inputCamera == null)
            {
                inputCamera = Camera.main;

#if UNITY_EDITOR
        //if (inputCamera == null) Debug.LogWarning(getStamp() + "no MainCamera tagged in context (frame : " + Time.frameCount + ")");
#endif

                return;
            }

            if (inputCamera == null)
            {
                inputCamera = transform.GetComponentInChildren<Camera>();
            }

            //camera tagged as 'input'
            if (inputCamera == null)
            {
                Camera[] cams = GameObject.FindObjectsOfType<Camera>();
                for (int i = 0; i < cams.Length; i++)
                {
                    if (inputCamera != null) continue;
                    if (isInLayerMask(cams[i].gameObject, _layer))
                    {
                        inputCamera = cams[i];
                        Debug.LogWarning("{InputTouchBridge} found a camera on 'input' layer");
                    }
                }
            }

        }

        /// <summary>
        /// never called by logic, meant to give dev something to reset all touch data
        /// </summary>
        public void reset()
        {
            pinchBridge.reset();

            //force active fingers to end state
            for (int i = 0; i < _fingers.Length; i++)
            {
                if (_fingers[i].isActiveAndUsed())
                {
                    _fingers[i].setEnded();
                }
            }

            //release all concerned fingers
            update_clean();
        }

        /// <summary>
        /// will only update when the setup is done
        /// </summary>
        void Update()
        {
            //solve all data of fingers
            //will change state and make fingers at "ended" when technically released
            if (isMobile()) update_touch();
            else update_desktop();

            update_clean(); // make all ended fingers as canceled

            update_callbacks(); // ontouch , onrelease

            pinchBridge.update(); // each frame
        }

        void update_callbacks()
        {
            for (int i = 0; i < _fingers.Length; i++)
            {

                //solve ...
                if (_fingers[i].phase == TouchPhase.Began)
                {
                    //Debug.Log("finger "+_fingers[i].fingerId+" <b>Began</b>");
                    if (onTouch != null) onTouch(_fingers[i]);
                }
                else if (_fingers[i].phase == TouchPhase.Moved || _fingers[i].phase == TouchPhase.Stationary)
                {
                    if (onOverring != null) onOverring(_fingers[i]);
                }

            }

            //each frame callback
            if (countFingers() > 0)
            {
                if (onTouching != null) onTouching();
            }
        }

        /// <summary>
        /// clean all finger that are setup as ended
        /// </summary>
        void update_clean()
        {

            for (int i = 0; i < _fingers.Length; i++)
            {
                if (_fingers[i].isPhaseEnded()) //forcekill done fingers
                {
                    killFinger(_fingers[i]);
                }
            }

        }

        /// <summary>
        /// this will trigger onRelease callback for that finger
        /// must be called once per end state
        /// </summary>
        /// <param name="finger"></param>
        void killFinger(InputTouchFinger finger)
        {
            //Debug.Log("killing " + finger.fingerId + " at phase " + finger.phase);

            //force a release if finger was used
            //active AND ended fingers !
            if (!finger.isPhaseCanceled())
            {
                if (onRelease != null) onRelease(finger); // on finger killed
            }

            finger.reset(); // set to canceled
        }

        //ON TOUCH DEVICES (MOBILE,TABLET)
        void update_touch()
        {
            touches = Input.touches; // stored for opti
            touchCount = Input.touchCount; // quantité de doigts a un instant T

            //first, check for new touch and assign touches to fingers
            for (int i = 0; i < touchCount; i++)
            {
                Touch touch = touches[i];
                _finger = getFingerById(touches[i].fingerId);
                if (_finger == null)
                {
                    _finger = getFirstAvailableFinger();
                    _finger.assign(touch.fingerId);
                }
            }

            //second, for each fingers apply state
            for (int i = 0; i < _fingers.Length; i++)
            {
                _finger = _fingers[i]; // never null

                Touch? touch = getSystemTouchById(_fingers[i].fingerId);
                if (touch != null) // active finger
                {
                    _finger.update(touch.Value);
                }
                else if (_finger.isFingerNotCanceled())
                {
                    _finger.setEnded();
                }
            }

        }

        //ON PC
        void update_desktop()
        {
            bool mouseDown = Input.GetMouseButton(0);

            touchCount = mouseDown ? 1 : 0;

            //default finger
            _finger = _fingers[0];

            if (mouseDown) _finger.update(0, Input.mousePosition);
            else if (_finger.isFingerNotCanceled()) _finger.setEnded();

        }

        /// <summary>
        /// returns Touch struct based on finger id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Touch? getSystemTouchById(int id)
        {
            //Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                if (touches[i].fingerId == id) return touches[i];
            }
            return null;
        }

        protected InputTouchFinger getFirstAvailableFinger()
        {
            for (int i = 0; i < _fingers.Length; i++)
            {
                if (_fingers[i].isPhaseCanceled()) return _fingers[i];
            }

            Debug.LogWarning("no available fingers ?");
            return null;
        }

        public bool hasTouchedCollider(Collider[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (hasTouchedCollider(list[i])) return true;
            }
            return false;
        }

        public bool hasTouchedCollider(Collider col)
        {
            for (int i = 0; i < _fingers.Length; i++)
            {
                if (!_fingers[i].isFingerNotCanceled()) continue;
                if (_fingers[i].hasCollider(col)) return true;
            }
            return false;
        }

        public Camera getInputCamera() { return inputCamera; }

        /* permet de récup un finger pas utilisé */
        protected InputTouchFinger getRecycledFinger()
        {
            for (int i = 0; i < _fingers.Length; i++)
            {
                if (_fingers[i].isPhaseCanceled()) return _fingers[i];
            }
            return null;
        }

        public InputTouchFinger[] getFingers() { return _fingers; }

        public InputTouchFinger getFingerById(int id, bool checkActivity = true)
        {
            for (int i = 0; i < _fingers.Length; i++)
            {
                //on retourne pas les doigts qui ont déjà fini
                //sur mobile la phase "ended" dure 6+ frames ...
                if (checkActivity && !_fingers[i].isFingerNotCanceled()) continue;

                if (_fingers[i].fingerId == id) return _fingers[i];
            }
            return null;
        }

        public int countFingers() { return touchCount; }
        public InputTouchFinger getFingerByIndex(int index) { return _fingers[index]; }

        public bool hasFingers() { return touchCount > 0; }
        public bool hasSelectedObject()
        {
            for (int i = 0; i < _fingers.Length; i++)
            {
                if (!_fingers[i].isFingerNotCanceled()) continue;
                if (_fingers[i].hasTouchedSomething()) return true;
            }
            return false;
        }

        //http://answers.unity3d.com/questions/150690/using-a-bitwise-operator-with-layermask.html
        public bool isColliderInteractive(Collider collider)
        {
            if (collider == null) return false;
            if ((_layer & (1 << collider.gameObject.layer)) > 0) return true;
            return false;
        }

        public string toString()
        {
            string content = "<color=red>[BRIDGE INPUT MANAGER]</color>";
            content += "\nisMobile() ? " + isMobile();

            content += "\ntouchCount   : " + touchCount + " / max " + _fingers.Length;

            for (int i = 0; i < _fingers.Length; i++)
            {
                if (_fingers[i].isFingerNotCanceled())
                {
                    content += "\n" + _fingers[i].toString();
                }
                else
                {
                    content += "\nfinger(" + i + ") is unused";
                }
            }

            return content;
        }

        /// <summary>
        /// return the name
        /// </summary>
        /// <returns></returns>
        public string getTouchedInteractorInfo()
        {
            string info = "";

            InputTouchFinger fing = _fingers[0];

            if (fing == null || fing.touchedObjects.Count == 0) return "\n\nNo touch";

            Collider2D col = fing.touchedObjects[0].collider;

            info += "\n\nTouch info : ";

            info += "\n" + col.name;

            info += "\n";
            return info;
        }

        static public bool isMobile()
        {
            if (Application.platform == RuntimePlatform.Android) return true;
            else if (Application.platform == RuntimePlatform.IPhonePlayer) return true;
            return false;
        }

#if UNITY_EDITOR
    Color gizmoColor = Color.red;
    void OnDrawGizmos()
    {
      Gizmos.color = gizmoColor;
      if (_fingers != null)
      {
        for (int i = 0; i < _fingers.Length; i++)
        {
          //Gizmos.DrawSphere(fingers[i].position, 0.5f);
          Gizmos.DrawSphere(_fingers[i].worldPosition, 0.1f);
        }
      }
    }
#endif

        [Header("debug stuff")]
        public bool useDebugInBuild = false;

        public bool drawDebug = false;

        Vector2 viewDimensions = new Vector2(Screen.width, Screen.height);
        protected GUIStyle style;

        void OnGUI()
        {
            if (!drawDebug) return;

            string ctx = toString();

            if (style == null)
            {
                style = new GUIStyle(GUI.skin.textArea);
                style.richText = true;
                style.normal.background = Texture2D.whiteTexture;
            }
            //style.normal.background

            //solve scaling

            float ratio = Screen.width / 720f;
            style.fontSize = Mathf.FloorToInt(ratio * 20f);

            viewDimensions.x = Screen.width;
            viewDimensions.y = Screen.height;

            //Vector2 dimensions = viewDimensions;
            //dimensions.y = dimensions.x / (Screen.width * 1f / Screen.height * 1f);

            //viewScaleFactor = Mathf.Max(viewScaleFactor, 0.1f);
            //Vector3 dim = new Vector3(Screen.width / (dimensions.x * viewScaleFactor), Screen.height / (dimensions.y * viewScaleFactor), 1);
            //GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, dim);

            Color black = Color.black;
            black.a = 0.8f;
            GUI.backgroundColor = black;

            guiDrawDebugInfo(0, ctx);

            if (pinchBridge != null)
            {
                guiDrawDebugInfo(1, pinchBridge.toString());
            }
        }

        protected void guiDrawDebugInfo(int windowIndex, string ctx)
        {
            //GUI.color = Color.red;
            float width = viewDimensions.x * 1f / (2f * 1.05f);
            float gap = 10f;

            //Debug.Log(viewDimensions.x+" , "+ width);

            GUI.Label(new Rect((gap * windowIndex) + (windowIndex * width), 10, width, Screen.height * 0.9f), ctx, style);
        }

        static public InputTouchFinger getDefaultFinger()
        {
            if (manager.countFingers() <= 0) return null;
            return get()._fingers[0];
        }

        static public Vector2 getDefaultFingerScreenPosition()
        {
            InputTouchFinger f = getDefaultFinger();
            if (f != null) return f.screenPosition;
            return Vector2.zero;
        }

        static private string getStamp()
        {
            return "<color=lightblue>InputTB</color> | ";
        }

        static public Vector2 getCursorPosition()
        {
            InputTouchBridge itb = InputTouchBridge.get();

            if (itb != null && itb.countFingers() > 0)
            {
                return getDefaultFinger().screenPosition;
            }

            return Input.mousePosition;
        }

        static protected InputTouchBridge manager;
        static public InputTouchBridge get()
        {

            //will create double if exist in later scenes (loading)
            //if (manager == null) manager = HalperComponentsGenerics.getManager<InputTouchBridge>("[input]");

            return manager;

        }

        //http://answers.unity3d.com/questions/150690/using-a-bitwise-operator-with-layermask.html
        static public bool isInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return ((layerMask.value & (1 << obj.layer)) > 0);
        }
        static public bool isInLayerMask(GameObject obj, int layerMask)
        {
            return obj.layer == layerMask;
        }

    }

}
