using System;
using System.Collections;
using ElementsIOT.General;
using TMPro;
using UnityEngine;

namespace ElementsIOT.IOT.Temperature
{
    public class TemperatureBehaviour : MonoBehaviour
    {
        #region VARIABLES

        [Header("Start OnEnable")]
        public bool StartImmediately = false;

        [Header("Temperature Text")]
        public TMP_Text TemperatureText;

        [Header("Connection")]
        public TemperatureConnection TemperatureConnection;
        public int LatestTimeDelay = 30;
        [Range(0f, 10f)]
        public float HistoricalPlaybackDelay = 1f;

        #endregion

        #region UNITY METHODS

        private void Start()
        {
            if (!StartImmediately) return;
            StartUpdatingLatestRecord();
        }

        private void OnEnable()
        {
            if (!StartImmediately) return;
            StartUpdatingLatestRecord();
        }

        #endregion

        #region METHODS

        public void StartUpdatingLatestRecord()
        {
            StopAllCoroutines();
            StartCoroutine(UpdateLatestRecord());
        }

        public void StartUpdatingHistoricalRecords()
        {
            StopAllCoroutines();
            StartCoroutine(UpdateHistoricalRecords());
        }

        private IEnumerator UpdateLatestRecord()
        {
            WaitForSeconds delay = new WaitForSeconds(LatestTimeDelay * Constants.SixtySeconds);
            while (gameObject.activeInHierarchy)
            {
                TemperatureConnection.UpdateLatestRecord(TemperatureText);
                yield return delay;
            }

            yield return null;
        }

        private IEnumerator UpdateHistoricalRecords()
        {
            WaitForSeconds delay = new WaitForSeconds(HistoricalPlaybackDelay);
            TemperatureConnection.UpdateHistoricalRecords();
            int length = TemperatureConnection.HistoricalRecords.Count;
            for (int i = 0; i < length; i++)
            {
                TemperatureText.SetText(TemperatureConnection.HistoricalRecords[i]);
                yield return delay;
            }

            yield return null;
            StartUpdatingLatestRecord();
        }

        #endregion
    }
}
