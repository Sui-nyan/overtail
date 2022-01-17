using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

namespace Overtail.GUI
{
    public class LoginMenu : MonoBehaviour
    {
        public Text status; // Status label
        public Button RegBtn; // Register button
        public Button LogBtn; // Login button

        public InputField MailField; // Email input field
        public InputField PassField; // Password input field

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
                        string jsonStr = await Overtail.API.POST("login", authData, false);
                        Dictionary<string, string> loginData =
                            JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);
                        Dictionary<string, string> pos =
                            JsonConvert.DeserializeObject<Dictionary<string, string>>(loginData["postition"]);

                        Overtail.API.Token = loginData["token"];    // Set token for API authorization
                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        var rb = player.gameObject.GetComponent<Rigidbody2D>();
                        rb.MovePosition(new Vector2(int.Parse(pos["x"]),
                            int.Parse(pos["y"])));                  // Move player to the saved position

                        status.text = "Welcome back!";
                        SceneManager.LoadScene(1);
                    }
                    catch (Exception)
                    {
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
