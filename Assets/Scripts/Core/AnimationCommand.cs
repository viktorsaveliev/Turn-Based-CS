using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Echobay
{
    [Serializable]
    public class AnimationCommand
    {
        [SerializeField, HorizontalGroup("Row1", Width = 0.6f)]
        [LabelText("Param")]
        private string _parameterName;

        [SerializeField, HorizontalGroup("Row1", Width = 0.4f)]
        [LabelText("Type")]
        private AnimatorParameterType _type;

        [ShowIf(nameof(IsInt)), SerializeField, LabelText("Int Value")]
        private int _intValue;

        [ShowIf(nameof(IsFloat)), SerializeField, LabelText("Float Value")]
        private float _floatValue;

        [ShowIf(nameof(IsBool)), SerializeField, LabelText("Bool Value")]
        private bool _boolValue;

        public string ParameterName => _parameterName;
        public AnimatorParameterType Type => _type;

        public void Apply(Animator animator)
        {
            switch (_type)
            {
                case AnimatorParameterType.Trigger:
                    animator.SetTrigger(_parameterName);
                    break;
                case AnimatorParameterType.Int:
                    animator.SetInteger(_parameterName, _intValue);
                    break;
                case AnimatorParameterType.Float:
                    animator.SetFloat(_parameterName, _floatValue);
                    break;
                case AnimatorParameterType.Bool:
                    animator.SetBool(_parameterName, _boolValue);
                    break;
            }
        }

        #region Odin Conditions
        private bool IsInt => _type == AnimatorParameterType.Int;
        private bool IsFloat => _type == AnimatorParameterType.Float;
        private bool IsBool => _type == AnimatorParameterType.Bool;
        #endregion
    }

    public enum AnimatorParameterType
    {
        Trigger,
        Int,
        Float,
        Bool
    }

}
