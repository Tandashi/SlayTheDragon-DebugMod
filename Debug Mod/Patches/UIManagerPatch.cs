#pragma warning disable CS0626

using MonoMod;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[MonoModPatch("global::UIManager")]
class UIManagerPatch : UIManager {

    public extern void orig_Start();
    public void Start() {
		orig_Start();


		this.UpdateDevPanelText();
		this.RemoveNonWorkingDevThings();
	}

	private void RemoveNonWorkingDevThings() {
		foreach(Image i in this.devPanel.GetComponentsInChildren<Image>()) {
			i.gameObject.SetActive(false);
        }
    }

	private void UpdateDevPanelText() {
		foreach (TextMeshProUGUI t in this.devPanel.GetComponentsInChildren<TextMeshProUGUI>()) {
			if (t.text.Contains("Dev stuff:")) {
				Vector3 pos = t.gameObject.transform.localPosition;
				t.gameObject.transform.localPosition = new Vector3(pos.x, pos.y + 18, pos.z);
				t.text = @"Keybinds:
CTRL + F1 - Cycle through checkpoints
CTRL + F2 - Go to last checkpoint
CTRL + F3 - 9999 health
CTRL + F4 - Fast forward toggle
CTRL + F5 - Faster movement toggle
CTRL + F6 - Third person view
CTRL + F7 - 10x attach power toggle
CTRL + F8 - Heart overlay toggle
CTRL + F9 - Sword toggle

ALT + B - Toggle Box Collider
ALT + C - Toggle Capsule Collider
ALT + S - Toggle Sphere Collider
ALT + M - Toggle Mesh Collider";
			}
		}
	}

}

