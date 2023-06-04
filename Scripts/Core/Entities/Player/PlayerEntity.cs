using PixelCrushers;
using SuspiciousGames.Saligia.Core.Entities.Components;
using SuspiciousGames.Saligia.Core.Entities.Components.Weapons;
using SuspiciousGames.Saligia.Core.Player;
using SuspiciousGames.Saligia.Core.Skills;
using SuspiciousGames.Saligia.UI;
using SuspiciousGames.Saligia.Utility;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

namespace SuspiciousGames.Saligia.Core.Entities.Player
{
    public class PlayerEntity : Entity
    {
        protected static PlayerEntity _instance;
        public static PlayerEntity Instance => _instance;

        #region Animator Paramter Hashes
        public static int StickTiltHash = Animator.StringToHash("StickTilt");
        public static int HorizontalHash = Animator.StringToHash("Horizontal");
        public static int VerticalHash = Animator.StringToHash("Vertical");

        public static int PrimaryAbilityTriggerHash = Animator.StringToHash("PrimaryAbility");
        public static int PrimaryAttack1TriggerHash = Animator.StringToHash("PrimaryAttack1");
        public static int PrimaryAttack2TriggerHash = Animator.StringToHash("PrimaryAttack2");
        public static int PrimaryAttack3TriggerHash = Animator.StringToHash("PrimaryAttack3");

        public static int SecondaryAbilityTriggerHash = Animator.StringToHash("SecondaryAbilityTrigger");

        public static int SecondaryAbilityAimingHash = Animator.StringToHash("SecondaryAbilityAiming");
        //Flying Orb Ability
        public static int FlyingOrbStartTriggerHash = Animator.StringToHash("FlyingOrbStartTrigger");
        //Scourge Ability
        public static int ScourgeStartTriggerHash = Animator.StringToHash("ScourgeStartTrigger");
        public static int EndScourgeTriggerHash = Animator.StringToHash("EndScourgeTrigger");
        #endregion

        #region Skills
        [Header("Secondary Abilities")]
        [SerializeField] private BaseSkill _secondaryAbilityOne;
        [SerializeField] private BaseSkill _secondaryAbilityTwo;
        [SerializeField] private BaseSkill _secondaryAbilityThree;

        [Space(10), Header("Spell Trail Renderers")]
        [SerializeField] private GameObject _dashTrailRenderer;
        [SerializeField] private GameObject _scytheTrailRenderer;
        [SerializeField] private GameObject _scytheComboTrailRenderer;

        public BaseSkill SecondaryAbilityOne => _secondaryAbilityOne;
        public BaseSkill SecondaryAbilityTwo => _secondaryAbilityTwo;
        public BaseSkill SecondaryAbilityThree => _secondaryAbilityThree;
        #endregion

        [field: SerializeField] public Canvas AbilityTargetingCanvas;
        public PlayerInventory PlayerInventory { get; private set; }

        [SerializeField] private float _maxActionRange;
        public float MaxActionRange => _maxActionRange;

        private Vector3 _moveVector;
        private Vector2 _input;

        private Camera _camera;
        private Targeting.TargetingComponent _targetingHelper;

        [SerializeField, Range(0.0f, 1.0f)] private float _runSpeedBorder;
        private float _animationMoveSpeedMultiplier = 1.0f;

        public Action<PlayerEntity> onDeath;

        [field: SerializeField] public GameObject ModelObject { get; private set; }

        private CheckTerrainTexture _checkTerrainTexture;

        private PlayerWeaponType _currentPlayerWeaponType;
        public PlayerWeaponType CurrentPlayerWeaponType => _currentPlayerWeaponType;

        public UnityEvent<SkillSlot, BaseSkill> onSkillChange;
        private static readonly int AnimationMoveSpeedMultiplier = Animator.StringToHash("AnimationMoveSpeedMultiplier");

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }

        protected override void OnStart()
        {
            PlayerInventory = GetComponent<PlayerInventory>();
            if (EnemyManager.Instance)
                EnemyManager.Instance.enemyDeathEvent.AddListener(PlayerInventory.RestorePotionCharges);
            _targetingHelper = GetComponent<Targeting.TargetingComponent>();
            _camera = Camera.main;
            _checkTerrainTexture = new CheckTerrainTexture(transform);
            SwitchWeapon(_currentPlayerWeaponType);
            onSkillChange.Invoke(SkillSlot.SecondaryOne, _secondaryAbilityOne);
            onSkillChange.Invoke(SkillSlot.SecondaryTwo, _secondaryAbilityTwo);
            onSkillChange.Invoke(SkillSlot.SecondaryThree, _secondaryAbilityThree);
            //PlayerInventory.events.potionEvents.onPotionSlotsChanged.Invoke();
            Time.timeScale = 1.0f;

            if (TryGetComponent(out PositionSaver positionSaver))
            {
                positionSaver.ApplyDataImmediate();
                positionSaver.enabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (MovementComponent == null || MovementComponent.Agent == null)
                return;
            
            _moveVector = new Vector3(_input.x, 0, _input.y);

            if (_input.magnitude < _runSpeedBorder && _input.magnitude > 0)// && !_targetingHelper.HasTarget)
                _animationMoveSpeedMultiplier = Mathf.Clamp(_input.magnitude / _runSpeedBorder, 0.4f, 1.0f);
            else
                _animationMoveSpeedMultiplier = Mathf.Clamp(_input.magnitude / 1.0f, _runSpeedBorder, 1.0f);

            Animator.SetFloat(AnimationMoveSpeedMultiplier, _animationMoveSpeedMultiplier * MovementComponent.CurrentMoveSpeed / MovementComponent.BaseMovementSpeed);

            if (_moveVector == Vector3.zero)
            {
                MovementComponent.Agent.enabled = false;
                return;
            }

            if (_targetingHelper.Target != null && !CastComponent.HasActiveSkill)
            {
                Vector3 temp = (_targetingHelper.Target.transform.position - transform.position).normalized;
                temp.y = 0;
                transform.forward = temp;
            }
            else
            {
                if (MovementComponent.CanRotate)
                {
                    var rot = _moveVector;
                    rot = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0) * rot;
                    if (rot != Vector3.zero)
                        MovementComponent.SetRotation(Quaternion.LookRotation(rot, Vector3.up));
                }
            }

            var moveVector = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0) * _moveVector * Time.fixedDeltaTime;

            MovementComponent.Move(moveVector);
        }

        #region Player InputActions
        public void OnMove(CallbackContext context)
        {
            _input = context.ReadValue<Vector2>();
            Animator.SetFloat(StickTiltHash, _input.magnitude);
            Animator.SetFloat(HorizontalHash, _input.x);
            Animator.SetFloat(VerticalHash, _input.y);
        }

        #region Abilities
        //Primary Ability 
        public void OnPrimaryAbility(CallbackContext context)
        {
            if (context.started)
            {
                WeaponComponent.AttackWithActiveWeapon();
                //TODO how do we handle all primary abilities
            }
        }

        //Secondary Ability one
        public void OnSecondaryAbilityOne(CallbackContext context)
        {

            if (_secondaryAbilityOne != null)
            {
                if (context.started)
                    CastComponent.Cast(_secondaryAbilityOne);
                if (context.canceled)
                    AimingComponent.EndAiming();
            }
        }

        public void OnSecondaryAbilityTwo(CallbackContext context)
        {
            if (_secondaryAbilityTwo != null)
            {
                if (context.started)
                    CastComponent.Cast(_secondaryAbilityTwo);
                if (context.canceled)
                    AimingComponent.EndAiming();
            }
        }

        public void OnSecondaryAbilityThree(CallbackContext context)
        {
            if (_secondaryAbilityThree != null)
            {
                if (context.started)
                    CastComponent.Cast(_secondaryAbilityThree);
                if (context.canceled)
                    AimingComponent.EndAiming();
            }
        }

        public void OnMovementAbility(CallbackContext context)
        {
            if (context.started)
            {
                WeaponComponent.CastMovementSkill();
            }
        }

        public void OnCorruptionAbility(CallbackContext context)
        {
            //if (_secondaryAbilityThree != null)
            //{
            //    if (context.started)
            //        CastComponent.Cast(_corruptionAbility);
            //    if (context.canceled)
            //        AimingComponent.EndAiming();
            //}
        }
        #endregion
        public void OnInteract(CallbackContext context)
        {
            //if (context.started)
            //    InputEvents.interactEvent.Invoke();
        }

        public void OnPotion_One(CallbackContext context)
        {
            if (context.started)
                PlayerInventory.UsePotion(PotionSlot.North);
        }

        public void OnPotion_Two(CallbackContext context)
        {
            if (context.started)
                PlayerInventory.UsePotion(PotionSlot.East);
        }

        public void OnPotion_Three(CallbackContext context)
        {
            if (context.started)
                PlayerInventory.UsePotion(PotionSlot.South);
        }

        public void OnPotion_Four(CallbackContext context)
        {
            if (context.started)
                PlayerInventory.UsePotion(PotionSlot.West);
        }

        public void OnToggle_WalkSprint(CallbackContext context)
        {
            //if (context.started)
            //    InputEvents.toggleWalkSprintEvent.Invoke();
        }
        #endregion

        public BaseSkill GetAbilityBySlot(SkillSlot skillSlot)
        {
            BaseSkill skill = null;
            switch (skillSlot)
            {
                case SkillSlot.Primary:
                    skill = WeaponComponent.GetActiveAttackSkill();
                    break;
                case SkillSlot.SecondaryOne:
                    skill = SecondaryAbilityOne;
                    break;
                case SkillSlot.SecondaryTwo:
                    skill = SecondaryAbilityTwo;
                    break;
                case SkillSlot.SecondaryThree:
                    skill = SecondaryAbilityThree;
                    break;
                case SkillSlot.Movement:
                    skill = WeaponComponent.GetActiveMovementSkill();
                    break;
            }
            return skill;
        }

        public void SetAbilityBySlot(BaseSkill skill, SkillSlot skillSlot)
        {
            if (skill == null)
                return;
            switch (skillSlot)
            {
                case SkillSlot.SecondaryOne:
                    _secondaryAbilityOne = skill;
                    break;
                case SkillSlot.SecondaryTwo:
                    _secondaryAbilityTwo = skill;
                    break;
                case SkillSlot.SecondaryThree:
                    _secondaryAbilityThree = skill;
                    break;
                default:
                    break;
            }
            onSkillChange.Invoke(skillSlot, skill);
        }

        public void ChangeSecondaryAbility(BaseSkill skill, SkillSlot skillSlot)
        {
            if (skillSlot == SkillSlot.Movement || skillSlot == SkillSlot.Primary)
                return;

            if (GetSkillSlotForSkill(skill, out SkillSlot equippedSkillSlot))
                SwapSkills(skillSlot, equippedSkillSlot);
            else
                SetAbilityBySlot(skill, skillSlot);
        }

        [Obsolete("Method is for UIElements only. Use SetAbilityBySlot instead")]
        public void ChangeSecondaryAbilityOld(BaseSkill skill, SkillSlot skillSlot)
        {
            if (skillSlot == SkillSlot.Movement)
                return;

            if (skillSlot == SkillSlot.Primary)
            {
                if (_currentPlayerWeaponType == PlayerWeaponType.Scythe)
                    SwitchWeapon(PlayerWeaponType.Grimoire);
                else
                    SwitchWeapon(PlayerWeaponType.Scythe);
            }
            else
            {
                if (GetSkillSlotForSkill(skill, out SkillSlot equippedSkillSlot))
                    SwapSkills(skillSlot, equippedSkillSlot);
                else
                    SetAbilityBySlot(skill, skillSlot);
            }
        }

        private bool GetSkillSlotForSkill(BaseSkill skill, out SkillSlot skillSlot)
        {
            if (skill == WeaponComponent.GetActiveAttackSkill())
            {
                skillSlot = SkillSlot.Primary;
                return true;
            }
            else if (skill == WeaponComponent.GetActiveMovementSkill())
            {
                skillSlot = SkillSlot.Movement;
                return true;
            }
            else if (skill == SecondaryAbilityOne)
            {
                skillSlot = SkillSlot.SecondaryOne;
                return true;
            }
            else if (skill == SecondaryAbilityTwo)
            {
                skillSlot = SkillSlot.SecondaryTwo;
                return true;
            }
            else if (skill == SecondaryAbilityThree)
            {
                skillSlot = SkillSlot.SecondaryThree;
                return true;
            }
            skillSlot = SkillSlot.Primary;
            return false;
        }

        private void SwapSkills(SkillSlot slot1, SkillSlot slot2)
        {
            BaseSkill tempSkill1;
            BaseSkill tempSkill2;

            tempSkill1 = GetAbilityBySlot(slot1);
            tempSkill2 = GetAbilityBySlot(slot2);

            SetAbilityBySlot(tempSkill1, slot2);
            SetAbilityBySlot(tempSkill2, slot1);
        }

        public void SwitchWeapon(PlayerWeaponType weaponType)
        {
            if (!WeaponComponent)
            {
                // this is only when a weapon switch is attempted before the weapon component is initialized
                _currentPlayerWeaponType = weaponType;
                return;
            }
            EquipWeapon(weaponType);
            if (!WeaponComponent.IsUnarmed)
            {
                onSkillChange.Invoke(SkillSlot.Primary, WeaponComponent.GetActiveAttackSkill());
                onSkillChange.Invoke(SkillSlot.Movement, WeaponComponent.GetActiveMovementSkill());
            }
        }

        public void Teleport(Vector3 targetPosition)
        {
            NavMesh.Raycast(transform.position, targetPosition, out var hit, NavMesh.AllAreas);
            MovementComponent.Warp(hit.position);
        }

        //This is important so the play can specify a movement direction of a movement ability
        public void ForceForward()
        {
            if (_input == Vector2.zero)
                return;

            var rot = new Vector3(_input.x, 0, _input.y);
            rot = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0) * rot;
            if (rot != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(rot, Vector3.up);
        }

        public void ActivateDashTrail(bool value)
        {
            _dashTrailRenderer.gameObject.SetActive(value);
        }

        public void ActivateScytheTrail(bool value, int attackSkillIndex = -1)
        {
            if (!value)
            {
                _scytheComboTrailRenderer.gameObject.SetActive(value);
                _scytheTrailRenderer.gameObject.SetActive(value);
                return;
            }
            if (attackSkillIndex == 2)
                _scytheComboTrailRenderer.gameObject.SetActive(value);
            else
                _scytheTrailRenderer.gameObject.SetActive(value);
        }

        private bool EquipWeapon(PlayerWeaponType weaponType)
        {
            switch (weaponType)
            {
                case PlayerWeaponType.Unarmed:
                    if (WeaponComponent.UnequipWeapon())
                    {
                        Animator.SetLayerWeight(1, 0);
                        Animator.SetLayerWeight(2, 0);
                        _currentPlayerWeaponType = PlayerWeaponType.Unarmed;
                        return true;
                    }
                    break;

                case PlayerWeaponType.Grimoire:
                    if (WeaponComponent.SwitchToWeapon(WeaponType.Ranged, "Grimoire"))
                    {
                        Animator.SetLayerWeight(1, 1);
                        Animator.SetLayerWeight(2, 0);
                        _currentPlayerWeaponType = PlayerWeaponType.Grimoire;
                        return true;
                    }
                    break;

                case PlayerWeaponType.Scythe:
                    if (WeaponComponent.SwitchToWeapon(WeaponType.Melee, "Scythe"))
                    {
                        Animator.SetLayerWeight(1, 0);
                        Animator.SetLayerWeight(2, 1);
                        _currentPlayerWeaponType = PlayerWeaponType.Scythe;
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        protected override void OnFootStepAnimationEvent()
        {
            if (!FootStepAudioPlayer || !MovementComponent || !MovementComponent.CanMove)
                return;

            if (_checkTerrainTexture.GetTerrainTexture())
            {
                foreach (var textureFloatPair in _checkTerrainTexture.TerrainTextureValues)
                    if (textureFloatPair.Value > 0)
                        FootStepAudioPlayer.PlayRandomClipOneShot(textureFloatPair.Key, textureFloatPair.Value);
            }
            else
            {
                FootStepAudioPlayer.PlayRandomClip();
            }
        }

        public void Respawn()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public override void OnDeath()
        {
            if (EnemyManager.Instance)
                EnemyManager.Instance.enemyDeathEvent.RemoveListener(PlayerInventory.RestorePotionCharges);
            AimingComponent.AbortAiming();
            WeaponComponent.ForceUnequip();
            Destroy(AimingComponent);
            Destroy(WeaponComponent);
            Destroy(CastCostComponent);
            Destroy(CastComponent);
            base.OnDeath();
            var ga = new GameActions();
            GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
            MovementComponent.Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            //TODO: game over code
            onDeath.Invoke(this);
        }
    }
}