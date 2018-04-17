using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {
	public Slider volumeSlider;

	public void VolumeControl(){
		AudioListener.volume = volumeSlider.value;
	}
}
