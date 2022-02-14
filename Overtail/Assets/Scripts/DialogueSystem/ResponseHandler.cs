using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Overtail.Dialogue
{
    /// <summary>
    /// Class handling reply options in conversations/dialgues.
    /// </summary>
    public class ResponseHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform responseBox;
        [SerializeField] private RectTransform responseButtonTemp;
        [SerializeField] private RectTransform responseContainer;

        private DialogueManager dialogueManager;
        private ResponseEvent[] responseEvents;

        private List<GameObject> tempResponseButtons = new List<GameObject>(); //temporary list of all the buttons created

        private void Start()
        {
            dialogueManager = GetComponent<DialogueManager>();
        }

        /// <summary>
        /// Sets currently possible responses
        /// </summary>
        /// <param name="responseEvents"></param>
        public void AddResponseEvents(ResponseEvent[] responseEvents)
        {
            this.responseEvents = responseEvents;
        }

        /// <summary>
        /// shows responses and makes all the options clickable, the height will be adjusted by the amount of options that are possible
        /// </summary>
        /// <param name="responses"></param>
        public void showResponses(Response[] responses)
        {
            float responseBoxHeight = 0;

            for (int i = 0; i < responses.Length; i++)
            {
                Response response = responses[i];
                int responseIndex = i;
                GameObject responseButton = Instantiate(responseButtonTemp.gameObject, responseContainer);
                responseButton.gameObject.SetActive(true);
                responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
                responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex));

                tempResponseButtons.Add(responseButton);

                responseBoxHeight += responseButtonTemp.sizeDelta.y;
            }

            responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
            responseBox.gameObject.SetActive(true);

        }

        /// <summary>
        /// Clears out all the options as soon as one is picked
        /// </summary>
        /// <param name="response"></param>
        /// <param name="responseIndex"></param>
        private void OnPickedResponse(Response response, int responseIndex)
        {
            responseBox.gameObject.SetActive(false);

            foreach (GameObject button in tempResponseButtons)
            {
                Destroy(button);
            }
            tempResponseButtons.Clear();

            if (responseEvents != null && responseIndex <= responseEvents.Length)
            {
                responseEvents[responseIndex].OnPickedResponse?.Invoke();
            }

            responseEvents = null;

            if (response.DialogueObject)
            {
                dialogueManager.StartDialogue(response.DialogueObject);
            }
            else
            {
                dialogueManager.CloseDialogue();
            }

        }
    }

}
