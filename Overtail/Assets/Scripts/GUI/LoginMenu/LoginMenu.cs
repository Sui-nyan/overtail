using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Overtail.PlayerModule;
using System.Threading.Tasks;
using System.Collections;

namespace Overtail.GUI
{
    public class LoginMenu : MonoBehaviour
    {
        struct LoginData
        {
            public string token;
            public Position position;

            public LoginData(string token, Position position)
            {
                this.token = token;
                this.position = position;
            }
        }

        struct Position
        {
            public float x;
            public float y;
            public string scene;

            public Position(float x, float y, string scene)
            {
                this.x = x;
                this.y = y;
                this.scene = scene;
            }
        }

        public Text status;             // Status label
        public Button RegBtn;           // Register button
        public Button LogBtn;           // Login button

        public InputField MailField;    // Email input field
        public InputField PassField;    // Password input field

        private void Start()
        {
            RegBtn.onClick.AddListener(Register);
            LogBtn.onClick.AddListener(Login);
        }

        private void Login() => Auth("login");
        private void Register() => Auth("register");

        private async void Auth(string action)
        {
            string mail = MailField.text;
            string pass = PassField.text;

            Dictionary<string, string> authData = new Dictionary<string, string> { { "email", mail }, { "password", pass } };

            switch (action)
            {
                case "login":
                    try
                    {
                        string jsonStr = await API.POST("login", authData, false);

                        LoginData loginData = JsonConvert.DeserializeObject<LoginData>(jsonStr);
                        API.Token = loginData.token;                                                    // Set token for API authorization

                        status.text = "Welcome back!";

                        var a = new Vector2(loginData.position.x, loginData.position.y);
                        Debug.Log(a);

                        IEnumerator LoadScene()
                        {
                            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loginData.position.scene); ;

                            // Wait until the asynchronous scene fully loads
                            while (!asyncLoad.isDone)
                            {
                                yield return null;
                            }

                            // TODO: This simply does not work but is not an error
                            Player player = GameObject.FindObjectOfType<Player>();
                            Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
                            rb.MovePosition(new Vector2(loginData.position.x, loginData.position.y));   // Move player to the saved position
                        }

                        StartCoroutine(LoadScene());

                        // TODO: This does not work here, move it to InventoryManager or sth
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                        status.text = "Login failed";
                    }

                    break;
                case "register":
                    try
                    {
                        await API.POST("register", authData, false);
                        status.text = "Registration successful, you can now log in :)";
                    }
                    catch (Exception)
                    {
                        status.text = "Registration failed, please try again";
                    }

                    break;
                default:
                    throw new ArgumentException("Undefined action was called");
            }
        }
    }
}
