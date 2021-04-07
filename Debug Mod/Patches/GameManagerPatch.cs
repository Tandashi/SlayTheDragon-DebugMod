#pragma warning disable CS0626

using MonoMod;
using UnityEngine;
using Modding.API;

[MonoModPatch("global::GameManager")]
class GameManagerPatch : GameManager {
	public bool dm_ThirdPersonToggle;
	public int  dm_CheckpointIndex;
	public bool dm_SwordToggle;
	public bool dm_SpeedToggle;
	public bool dm_FastForwardToggle;
	public bool dm_PowerToggle;
	public bool dm_HeartsToggle;

	private bool dm_DrawingBoxColliders = false;
	private bool dm_DrawingCapsuleColliders = false;
	private bool dm_DrawingSphereColliders = false;
	private bool dm_DrawingMeshColliders = false;

	public extern void orig_Update();
    public void Update() {
        orig_Update();

		if (Input.GetKeyDown(KeyCode.Backspace)) {
			Managers.UIManager.ShowDevPanel(!Managers.UIManager.devPanel.activeSelf);
		}

        #region Control + ?  Binds
        if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) { 
			if (Input.GetKeyDown(KeyCode.F1) && Managers.CheckpointManager.checkpointsInScene.Count > 0) {
				this.dm_CheckpointIndex++;
				Player.Instance.transform.position = Managers.CheckpointManager.checkpointsInScene[this.dm_CheckpointIndex].transform.position;
				if (this.dm_CheckpointIndex >= Managers.CheckpointManager.checkpointsInScene.Count - 1) {
					this.dm_CheckpointIndex = -1;
				}
			}

			if (Input.GetKeyDown(KeyCode.F2)) {
				Player.Instance.transform.position = Managers.CheckpointManager.lastCheckpoint;
			}

			if (Input.GetKeyDown(KeyCode.F3)) {
				PlayerAPI.SetCurrentPlayerHealth(9999);
				PlayerAPI.SetPlayerCanTakeDamage(true);
			}

			if (Input.GetKeyDown(KeyCode.F4)) {
				this.dm_FastForwardToggle = !this.dm_FastForwardToggle;
				Time.timeScale = this.dm_FastForwardToggle ? 5 : 1;
			}

			if (Input.GetKeyDown(KeyCode.F5)) {
				this.dm_SpeedToggle = !this.dm_SpeedToggle;
				float speed = Player.Instance.Stats.DefaultMoveSpeed;
				PlayerAPI.SetMoveSpeed(this.dm_SpeedToggle ? speed * 2.2f : speed);
			}

			if (Input.GetKeyDown(KeyCode.F6)) {
				if (this.dm_ThirdPersonToggle) {
					this.dm_ThirdPersonToggle = false;
					ThirdPersonAPI.DeactivateThirdPerson();
				}
				else {
					this.dm_ThirdPersonToggle = true;
					ThirdPersonAPI.ActivateThirdPerson();
				}
			}

			if (Input.GetKeyDown(KeyCode.F7)) {
				this.dm_PowerToggle = !this.dm_PowerToggle;
				PlayerAPI.SetDamage(this.dm_PowerToggle ? 10 : 1);
			}

			if (Input.GetKeyDown(KeyCode.F8)) {
				this.dm_HeartsToggle = !this.dm_HeartsToggle;
				Managers.UIManager.ToggleHearts(this.dm_HeartsToggle);
			}

			if (Input.GetKeyDown(KeyCode.F9) && Player.Instance != null) {
				if (this.dm_SwordToggle) {
					PlayerAPI.DisableSword();
					this.dm_SwordToggle = false;
				}
				else {
					PlayerAPI.EnableSword();
					this.dm_SwordToggle = true;
				}
			}
		}
        #endregion

        #region ALT + ? Binds
		if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) {
			if (Input.GetKeyDown(KeyCode.B)) {
				this.dm_DrawingBoxColliders = !this.dm_DrawingBoxColliders;
				if (this.dm_DrawingBoxColliders)
					BoundsAPI.DrawBoxColliders(FindObjectsOfType<BoxCollider>());
				else
					BoundsAPI.DestroyBounds(BoundsType.BOX_COLLIDER);
			}

			if (Input.GetKeyDown(KeyCode.C)) {
				this.dm_DrawingCapsuleColliders = !this.dm_DrawingCapsuleColliders;
				if (this.dm_DrawingCapsuleColliders)
					BoundsAPI.DrawCapsuleColliders(FindObjectsOfType<CapsuleCollider>());
				else
					BoundsAPI.DestroyBounds(BoundsType.CAPSULE_COLLIDER);
			}

			if (Input.GetKeyDown(KeyCode.S)) {
				this.dm_DrawingSphereColliders = !this.dm_DrawingSphereColliders;
				if (this.dm_DrawingSphereColliders)
					BoundsAPI.DrawSphereColliders(FindObjectsOfType<SphereCollider>());
				else
					BoundsAPI.DestroyBounds(BoundsType.SPHERE_COLLIDER);
			}

			if (Input.GetKeyDown(KeyCode.M)) {
				this.dm_DrawingMeshColliders = !this.dm_DrawingMeshColliders;
				if (this.dm_DrawingMeshColliders)
					BoundsAPI.DrawMeshColliders(FindObjectsOfType<MeshCollider>());
				else
					BoundsAPI.DestroyBounds(BoundsType.MESH_COLLIDER);
			}
		}
        #endregion
    }
}

