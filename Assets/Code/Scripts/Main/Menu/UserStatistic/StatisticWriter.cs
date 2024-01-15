using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ITPRO.Rover.User
{
    public class StatisticWriter : MonoBehaviour
    {
        public static StatisticWriter instance;

        private RoverStatistic _statistic;

        public static User ActiveUser { get; set; }
        public static GameDifficult ActiveDifficult { get; set; }
        
        private string _fileName;
        private static string _path;

        private void Awake()
        {
            _path = Application.dataPath + @"/Statistic";
            
            if (instance) Destroy(this);
            else
            {
                instance = this;
                _path = Application.dataPath + @"/Statistic";
            }
        }

        private void Start()
        {
            DateTime date = DateTime.Now;            
            _fileName = $"{_statistic.id}_{date:dd.MM.yy}.json";
            int i = 1;
            while (File.Exists(_path + '/' + _fileName))
            {
                _fileName = $"{_statistic.id}_{date:dd.MM.yy}_{i}.json";
                i++;
                if (i > 100000)
                {
                    break;
                }
            }
        }

        private void OnEnable()
        {
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

            ActiveUser ??= new User();
            _statistic = new RoverStatistic(ActiveUser.ID, ActiveDifficult)
            {
                attempt = 1 + Directory.GetFiles(_path, $"{ActiveUser.ID}_*.json").Length
            };
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Input.GetKeyDown(KeyCode.G)) return;
            SaveJson();
#endif
        }  

        public void SaveJson()
        {
            if (_fileName == null) return;
            string jsonSring = JsonUtility.ToJson(_statistic, true);
            File.WriteAllText(_path + '/' + _fileName, jsonSring);
        }

        public int Attempt
        {
            get => _statistic.attempt;
            set => _statistic.attempt = value;
        }

        public bool Succeeded
        {
            get => _statistic.succeeded;
            set => _statistic.succeeded = value;
        }
        
        public string FailReason
        {
            get => _statistic.failReason;
            set => _statistic.failReason = value;
        }


        public string ExecutionDuration
        {
            get => _statistic.executionDuration;
            set => _statistic.executionDuration = value;
        }

        public GameDifficult Difficult
        {
            get => _statistic.difficult;
            set => _statistic.difficult = value;
        }

        public string StartTime
        {
            get => _statistic.startTime;
            set => _statistic.startTime = value;
        }

        public float FirstTraveledDistance
        {
            get => _statistic.firstTraveledDistance;
            set => _statistic.firstTraveledDistance = value;
        }

        public float SecondTraveledDistance
        {
            get => _statistic.secondTraveledDistance;
            set => _statistic.secondTraveledDistance = value;
        }

        public float ThirdTraveledDistance
        {
            get => _statistic.thirdTraveledDistance;
            set => _statistic.thirdTraveledDistance = value;
        }

        public string ActiveFirstPoint
        {
            get => _statistic.activeFirstPoint;
            set => _statistic.activeFirstPoint = value;
        }

        public string ActiveSecondPoint
        {
            get => _statistic.activeSecondPoint;
            set => _statistic.activeSecondPoint = value;
        }

        public string ActiveThirdPoint
        {
            get => _statistic.activeThirdPoint;
            set => _statistic.activeThirdPoint = value;
        }

        public int CollectionSamples
        {
            get => _statistic.collectionSamples;
            set => _statistic.collectionSamples = value;
        }

        public string CollectionDuration
        {
            get => _statistic.collectionDuration;
            set => _statistic.collectionDuration = value;
        }

        public string SmallMineral
        {
            get => _statistic.smallMineral;
            set => _statistic.smallMineral = value;
        }

        public string MediumMineral
        {
            get => _statistic.mediumMineral;
            set => _statistic.mediumMineral = value;
        }

        public string BigMineral
        {
            get => _statistic.bigMineral;
            set => _statistic.bigMineral = value;
        }

        public int ControlErrors
        {
            get => _statistic.controlErrors;
            set => _statistic.controlErrors = value;
        }

        public string CompletionDocking
        {
            get => _statistic.completionDocking;
            set => _statistic.completionDocking = value;
        }

        public string ChargeFirstPoint
        {
            get => _statistic.chargeFirstPoint;
            set => _statistic.chargeFirstPoint = value;
        }

        public string ChargeSecondPoint
        {
            get => _statistic.chargeSecondPoint;
            set => _statistic.chargeSecondPoint = value;
        }

        public string ChargeThirdPoint
        {
            get => _statistic.chargeThirdPoint;
            set => _statistic.chargeThirdPoint = value;
        }

        public RdoAttempt FirstRdo
        {
            get => _statistic.firstRdo;
            set => _statistic.firstRdo = value;
        }

        public RdoAttempt SecondRdo
        {
            get => _statistic.secondRdo;
            set => _statistic.secondRdo = value;
        }

        public RdoAttempt ThirdRdo
        {
            get => _statistic.thirdRdo;
            set => _statistic.thirdRdo = value;
        }

        public void AddWheelAttempt(WheelAttempt attempt)
        {
            attempt.index = _statistic.wheelAttempts.Count + 1;

            _statistic.wheelAttempts.Add(attempt);
        }
        
        public void AddRavenAttempt(RavenAttempt attempt)
        {
            attempt.index = _statistic.ravenAttempts.Count + 1;

            _statistic.ravenAttempts.Add(attempt);
        }
        
        public void AddFrame(ControllerFrame frame)
        {
            _statistic.controllerFrames.Add(frame);
        }

        public void AddRdoAttempt(int number, RdoAttempt attempt)
        {
            switch (number)
            {
                case 0:
                    _statistic.firstRdo = attempt;
                    break;
                case 1:
                    _statistic.secondRdo = attempt;
                    break;
                case 2:
                    _statistic.thirdRdo = attempt;
                    break;
            }
        }
    }
}