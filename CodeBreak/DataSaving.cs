﻿// The purpose of this class is to store the names and scores of players into PlayerDatabase.

// If the player is a new player, their score is accepted as a high score and their data will be saved into PlayerDatabase as a new record.

// If the player is a returning player, their score will be compared to their previous high score. If their score is higher than their

// previous high score, their record in PlayerDatabase will be updated with the new high score, but if their score is lower than their
// previous high score, no new data will be saved.

using System.Collections.ObjectModel;

namespace CodeBreak
{
    public class DataSaving
    {
        private bool _IsHighScore;
        public DataSaving()
        {
            _IsHighScore = true;
        }

        public async void SaveEasyPlayerData()
        {
            var players = await PlayerDatabase.GetEasyPlayersByScore();
            ObservableCollection<EasyPlayers> easyList = new ObservableCollection<EasyPlayers>(players);
            EasyPlayers easyRecord = new EasyPlayers();

            foreach (var player in easyList)
            {
                if (App.CurrentName == player.Name)
                {
                    if (App.CurrentScore > player.Score)
                        easyRecord.RowId = player.RowId;
                    else
                        _IsHighScore = false;

                    break;
                }
            }

            if (_IsHighScore == true && App.CurrentScore > 0)
            {
                easyRecord.Name = App.CurrentName;
                easyRecord.Score = App.CurrentScore;
                await PlayerDatabase.SaveEasyPlayerAsync(easyRecord);
            }

            App.CurrentName = null;
            App.CurrentDifficulty = null;
            App.CurrentScore = 0;
        }

        public async void SaveNormalPlayerData()
        {
            var players = await PlayerDatabase.GetNormalPlayersByScore();
            ObservableCollection<NormalPlayers> normList = new ObservableCollection<NormalPlayers>(players);
            NormalPlayers normRecord = new NormalPlayers();


            foreach (var player in normList)
            {
                if (App.CurrentName == player.Name)
                {
                    if (App.CurrentScore > player.Score)
                        normRecord.RowId = player.RowId;
                    else
                        _IsHighScore = false;
                    break;
                }
            }

            if (_IsHighScore == true && App.CurrentScore > 0)
            {
                normRecord.Name = App.CurrentName;
                normRecord.Score = App.CurrentScore;
                await PlayerDatabase.SaveNormalPlayerAsync(normRecord);
            }

            App.CurrentName = null;
            App.CurrentDifficulty = null;
            App.CurrentScore = 0;
        }

        public async void SaveHardPlayerData()
        {
            var players = await PlayerDatabase.GetHardPlayersByScore();
            ObservableCollection<HardPlayers> hardList = new ObservableCollection<HardPlayers>(players);
            HardPlayers hardRecord = new HardPlayers();


            foreach (var player in hardList)
            {
                if (App.CurrentName == player.Name)
                {
                    if (App.CurrentScore > player.Score)
                        hardRecord.RowId = player.RowId;
                    else
                        _IsHighScore = false;

                    break;
                }
            }

            if (_IsHighScore == true && App.CurrentScore > 0)
            {
                hardRecord.Name = App.CurrentName;
                hardRecord.Score = App.CurrentScore;
                await PlayerDatabase.SaveHardPlayerAsync(hardRecord);
            }

            App.CurrentName = null;
            App.CurrentDifficulty = null;
            App.CurrentScore = 0;
        }

        public async void SaveImpossiblePlayerData()
        {
            var players = await PlayerDatabase.GetImpossiblePlayersByScore();
            ObservableCollection<ImpossiblePlayers> impList = new ObservableCollection<ImpossiblePlayers>(players);
            ImpossiblePlayers impRecord = new ImpossiblePlayers();


            foreach (var player in impList)
            {
                if (App.CurrentName == player.Name)
                {
                    if (App.CurrentScore > player.Score)
                        impRecord.RowId = player.RowId;
                    else
                        _IsHighScore = false;

                    break;
                }
            }

            if (_IsHighScore == true && App.CurrentScore > 0)
            {
                impRecord.Name = App.CurrentName;
                impRecord.Score = App.CurrentScore;
                await PlayerDatabase.SaveImpossiblePlayerAsync(impRecord);
            }

            App.CurrentName = null;
            App.CurrentDifficulty = null;
            App.CurrentScore = 0;
        }
    }
}
