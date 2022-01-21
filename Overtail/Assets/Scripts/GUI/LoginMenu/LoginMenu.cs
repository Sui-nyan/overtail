using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Overtail.PlayerModule;
using System.Threading.Tasks;
using System.Collections;
using TMPro;

namespace Overtail.GUI
{
    public class LoginMenu : MonoBehaviour
    {
        private struct LoginData
        {
            public string token;
            public Position position;
        }

        private struct Position
        {
            public float x;
            public float y;
            public string scene;
        }

        [SerializeField] private TMP_Text status;           // Status label
        [SerializeField] private Button regBtn;             // Register button
        [SerializeField] private Button logBtn;             // Login button

        [SerializeField] private TMP_InputField mailField;  // Email input field
        [SerializeField] private TMP_InputField passField;  // Password input field

        private void Start()
        {
            regBtn.onClick.AddListener(Register);
            logBtn.onClick.AddListener(Login);
        }

        private void Login() => Auth("login");
        private void Register() => Auth("register");

        private async void Auth(string action)
        {
            string mail = mailField.text;
            string pass = passField.text;

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
                            Player player = FindObjectOfType<Player>();
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
