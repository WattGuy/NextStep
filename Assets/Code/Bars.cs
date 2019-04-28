using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{

    public static Bars instance;
    public static bool isRunning = false;
    public bool cor_started = false;
    private static int time = 600;

    void Start()
    {
        instance = this;
        if (ContentManager.data == null) ContentManager.data = Saver.load();

        update();
    }

    public IEnumerator timer() {
        while (true)
        {
            if (!isRunning) {

                foreach (Transform child in GameObject.FindGameObjectWithTag("Energy").transform.parent)
                    if (child.name == "Time") child.gameObject.SetActive(false);

                cor_started = false;
                StopCoroutine(timer());
                yield break;

            }
            long diff = (ContentManager.data.complete - DateTime.Now.Ticks) / TimeSpan.TicksPerSecond;
            long gdiff = (ContentManager.data.complete - ContentManager.data.time) / TimeSpan.TicksPerSecond;
            int number = Math.Abs((int) gdiff / time);

            if (ContentManager.data.complete <= DateTime.Now.Ticks) {

                isRunning = false;

                if (ContentManager.data.energy + number >= ContentManager.data.energy_max) {

                    ContentManager.data.energy = ContentManager.data.energy_max;

                }else ContentManager.data.energy += number;

                ContentManager.data.time = 0;
                ContentManager.data.complete = 0;
                ContentManager.data.last_reward = 0;

                Saver.save(ContentManager.data);

                foreach (Transform child in GameObject.FindGameObjectWithTag("Energy").transform.parent)
                    if (child.name == "Time") child.gameObject.SetActive(false);

                cor_started = false;
                update();
                StopCoroutine(timer());
                yield break;

            }

            int all = number * time;

            for (int i = 1; i <= number; i++) {
                all -= time;
                if (i <= ContentManager.data.last_reward) continue;

                if (all >= diff) {

                    ContentManager.data.last_reward += 1;
                    ContentManager.data.energy += 1;
                    update();

                }

            }

            Saver.save(ContentManager.data);

            TimeSpan t = TimeSpan.FromSeconds(diff % time);
            foreach (Transform child in GameObject.FindGameObjectWithTag("Energy").transform.parent)
                if (child.name == "Time") {

                    child.gameObject.SetActive(true);
                    child.GetComponent<Text>().text = t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");

                }

            yield return new WaitForSeconds(1f);
        }
    }

    public static void checkTimer() {

        if (ContentManager.data.energy >= ContentManager.data.energy_max)
        {

            ContentManager.data.time = 0;
            ContentManager.data.complete = 0;
            ContentManager.data.last_reward = 0;
            Saver.save(ContentManager.data);
            isRunning = false;

        }
        else {

            if (ContentManager.data.time == 0) {

                ContentManager.data.time = DateTime.Now.Ticks;
                ContentManager.data.complete = DateTime.Now.Ticks + (TimeSpan.TicksPerSecond * (time * (ContentManager.data.energy_max - ContentManager.data.energy)));
                ContentManager.data.last_reward = 0;
                Saver.save(ContentManager.data);

            }
            
            isRunning = true;

            if (!instance.cor_started) {
                instance.StartCoroutine(instance.timer());
                instance.cor_started = true;
            }

        }

    }

    public static void update() {

        GameObject.FindGameObjectWithTag("Energy").GetComponent<Text>().text = ContentManager.data.energy + "/" + ContentManager.data.energy_max;
        GameObject.FindGameObjectWithTag("Coins").GetComponent<Text>().text = ContentManager.data.coins.ToString();
        GameObject.FindGameObjectWithTag("Keys").GetComponent<Text>().text = ContentManager.data.keys.ToString();

        checkTimer();

    }

}
