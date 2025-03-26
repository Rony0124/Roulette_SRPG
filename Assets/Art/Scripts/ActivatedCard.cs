using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;

public class ActivatedCard : MonoBehaviour
{
   [SerializeField] private MMFeedbacks feedback;
   
   public float FeelDuration => feedback.TotalDuration;

   public void MoveFeedback()
   {
      feedback.PlayFeedbacks();
   }
}
