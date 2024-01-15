using ITPRO.Rover.Controller;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ITPRO.Rover.User
{
    [Serializable]
    public class RoverStatistic
    {
        public RoverStatistic(int userID, GameDifficult difficult)
        {
            id = userID;
            this.difficult = difficult;

            ravenAttempts = new List<RavenAttempt>();
            wheelAttempts = new List<WheelAttempt>();
            controllerFrames = new List<ControllerFrame>();
        }

        public int id;
        public int attempt;
        public bool succeeded;
        public string failReason;
        public string executionDuration;

        public GameDifficult difficult;
        public string startTime;

        public float firstTraveledDistance;
        public float secondTraveledDistance;
        public float thirdTraveledDistance;

        public string activeFirstPoint;
        public string activeSecondPoint;
        public string activeThirdPoint;
        
        public string chargeFirstPoint;
        public string chargeSecondPoint;
        public string chargeThirdPoint;
        
        public int collectionSamples;
        public string collectionDuration;
        public string smallMineral = "пропущен";
        public string mediumMineral = "пропущен";
        public string bigMineral = "пропущен";
        public int controlErrors = 0;

        public string completionDocking;

        public RdoAttempt firstRdo;
        public RdoAttempt secondRdo;
        public RdoAttempt thirdRdo;

        public List<RavenAttempt> ravenAttempts;
        public List<WheelAttempt> wheelAttempts;
        public List<ControllerFrame> controllerFrames;
    }

    [Serializable]
    public struct RdoAttempt
    {
        public string startTime;
        public string endTime;
        public int successfulAttempts;
        public string executionTime;
        public List<string> deviations;
    }
    
    public enum GameDifficult
    {
        Easy,
        Medium,
        Hard
    }

    [Serializable]
    public struct RavenAttempt
    {
        public int index;
        public string startTime;
        public string endTime;
        public float reliabilityFactor;
        public float speedFactor;
    }

    [Serializable]
    public struct WheelAttempt
    {
        public int index;
        public bool firstError;
        public bool secondError;
        public string detectionTime;
        public string correctionTime;
        public string executionTime;
    }

    [Serializable]
    public struct ControllerFrame
    {
        public int index;
        public Vector2 joy;
        public float speed;
        public float charge;
        public float pitch; //тангаж
        public float roll; //крен
        public Vector3 pos;
    }
}