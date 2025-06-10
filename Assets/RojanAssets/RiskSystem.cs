using UnityEngine;

public class RiskSystem : MonoBehaviour
{
    public enum RiskLevel { Low, Medium, High }

    public RiskLevel currentRiskLevel = RiskLevel.Medium; // Default risk level

    // Adjust the bet based on the selected risk level
    public float CalculateRiskedAmount(int betAmount)
    {
        switch (currentRiskLevel)
        {
            case RiskLevel.Low:
                return betAmount * 1.0f; // No multiplier
            case RiskLevel.Medium:
                return betAmount * 1.5f; // 1.5x multiplier
            case RiskLevel.High:
                return betAmount * 2.0f; // 2x multiplier
            default:
                return betAmount;
        }
    }
}
