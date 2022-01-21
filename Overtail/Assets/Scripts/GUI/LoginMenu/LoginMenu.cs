using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using TMPro;

namespace Overtail.GUI
{
    public class LoginMenu : MonoBehaviour
    {
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
                        Debug.Log(jsonStr);

                        var loginData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonStr);

                        API.Token = loginData["token"];     // Set token for API authorization
                        status.text = "Welcome back!";

                        SceneManager.LoadScene(loginData["scene"] ?? "OverworldScene");
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
