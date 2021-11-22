using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using org.mariuszgromada.math.mxparser;

public class lvlController : MonoBehaviour
{
    public Text _fpsText;
    private float _hudRefreshRate = 1f;
    private float _timer;

    public float point;
    [Range(0f, 1f)]
    public Vector3 desiredPosition;
    public Vector3 evenTextStartPos;
    public Vector3 oddTextStartPos;
    public Vector2 startPos;
    public Vector2 direction;
    public string exp;
    public Text expText;
    public Text oddText;
    public Text evenText;
    public string tanswer;
    public string answer;
    public float smoothSpeed = 5f;
    public bool untouched;
    public Color color;
    public Color startcolor;

    lvlMechanics lvl = new lvlMechanics();
    Generator generator = new Generator();

    // Start is called before the first frame update
    void Start()
    {
        color = oddText.color;
        startcolor = oddText.color;
        exp = generator.Generate(lvl.getLvL());
        expText.text = exp;
        Expression expression = new Expression(exp);
        tanswer = expression.calculate() % 2 == 0 ? "even" : "odd";
        answer = "";
        evenTextStartPos = evenText.transform.position;
        oddTextStartPos = oddText.transform.position;
        desiredPosition.y = evenTextStartPos.y;
}

    private void FixedUpdate()
    { 

    }
    void Update()
    {


        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    Debug.LogError("reached2");
                    // Record initial touch position.
                    startPos = touch.position;
                    untouched = false;
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    direction = touch.position - startPos;
                    desiredPosition.x = 0.0f;
                    if (direction.x > 105.0f) startPos.x += 5.0f;
                    if (direction.x < -105.0f) startPos.x -= 5.0f;
                        if (direction.x > 0)
                        {
                            evenText.transform.position = Vector3.Lerp(evenText.transform.position, evenTextStartPos, smoothSpeed * Time.deltaTime);
                            
                            point = Mathf.Abs(direction.x / 100);


                        if (direction.x > 100)
                            {
                                point = 1;
                                answer = "even";
                            }
                            else
                            {
                                answer = "";
                            }
                        oddText.transform.position = Vector3.Lerp(oddTextStartPos, desiredPosition, point);
                        }

                        if (direction.x < 0)
                        {
                            oddText.transform.position = Vector3.Lerp(oddText.transform.position, oddTextStartPos, smoothSpeed * Time.deltaTime);
                            point = Mathf.Abs(direction.x / 100);
                            if (direction.x < -100)
                            {
                                point = 1;
                                answer = "odd";
                            }
                            else
                            {
                                answer = "";
                            }
                         evenText.transform.position = Vector3.LerpUnclamped(evenTextStartPos, desiredPosition, point);
                    }
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    if (answer == "")
                    {
                        untouched = true;
                    } 
                    else
                    {
                        
                        if (tanswer == answer) Debug.LogError("Win");
                        else Debug.LogError("Lose");
                        untouched = true;
                        NextLevel();
                    }
                    break;   
            }  
        }
        point = 1 - (-oddText.transform.position.x / evenTextStartPos.x);
        color.a = 1f - (point * 3f - 0.1f);
        evenText.color = color;
        point = 1 - (evenText.transform.position.x / evenTextStartPos.x);
        color.a = 1f - (point * 3f - 0.1f);
        oddText.color = color;
        if (untouched)
        {
            evenText.transform.position = Vector3.Lerp(evenText.transform.position, evenTextStartPos, smoothSpeed * Time.deltaTime);
            oddText.transform.position = Vector3.Lerp(oddText.transform.position, oddTextStartPos, smoothSpeed * Time.deltaTime);

        }
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = "FPS: " + fps + "d:  " + direction.x;
            _timer = Time.unscaledTime + _hudRefreshRate;
        }
    }

    void NextLevel()
    {
        lvl.lvlUp();
        exp = generator.Generate(lvl.getLvL());
        expText.text = exp;
        Expression expression = new Expression(exp);
        tanswer = expression.calculate() % 2 == 0 ? "even" : "odd";
        answer = "";
    }
}
