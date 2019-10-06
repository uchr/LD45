using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI = UnityEngine.UI;

public class Tutorial : MonoBehaviour {
    private string help0 = "<b>Зажмите пробел чтобы воскресить мёртвых.</b>";

    private float time1 = 3.0f;
    private string help1 = "<b>Зажмите правую кнопку мыши чтобы защитится.</b>";

    private float time2 = 3.0f;
    private string help2 = "<b>Зажмиту левую кнопку мыши чтобы атаковать и грабить.</b>";

    private float time3 = 3.0f;
    private string help3 = "<b>Нужно три воскрешонных чтобы уничтожить один дом.</b>";

    private float time4 = 3.0f;
    private string help4 = "<b>Если у вас закончится армия - вернитесь на кладбище.</b>";

    private float time5 = 3.0f;
    private string help5 = "<b>Пора уничтожить эту город.</b>";

    private float time6 = 1000000000.0f;
    private string help6 = "";

    public UI.Text text;

    public float timer = 0.0f;

    public int stage = 0;

    private List<float> times = new List<float>();
    private List<string> helps = new List<string>();

    public void Awake() {
        text.text = help0;

        times.Add(1000000.0f);
        times.Add(time1);
        times.Add(time2);
        times.Add(time3);
        times.Add(time4);
        times.Add(time5);
        times.Add(time6);

        helps.Add(help0);
        helps.Add(help1);
        helps.Add(help2);
        helps.Add(help3);
        helps.Add(help4);
        helps.Add(help5);
        helps.Add(help6);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (stage < times.Count && timer > times[stage]) {
            NextStage();
        }
    }

    public void NextStage() {
        timer = 0.0f;
        ++stage;
        text.text = helps[stage];
    }

    public void ResetTimer() {
        timer = 0.0f;
    }
}
