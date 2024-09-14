using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace Quaze.Models;



public class Session
{
    public string Id { get; private set; }

    public string OwnerId { get; private set; }

    public List<Participant> Participants { get; set; } = new();

    public int NumberOfAnswersSent { get; private set; } = 0;
    public int TimeLeft { get; private set; } = 0;
    public int QuestionIndex { get; private set; } = 0;
    public SessionState State { get; private set; }

    public Question CurrentQuestion => Quiz.Questions[QuestionIndex];

    public event EventHandler<SessionState> StateChanged;
    public event EventHandler<int> TimerTick;

    public Session(string sessionId, string userId, Quiz quiz) {
        Id = sessionId;
        Quiz = quiz;
        OwnerId = userId;
    }

    public Quiz Quiz {get; private set;}
    private bool TimerEnabled = false;

    public void OnTimerTick() {
        if (!TimerEnabled)
        {
            return;
        }

        if (TimeLeft > 0)
        {
            TimeLeft--;
            TimerTick?.Invoke(this, TimeLeft);
            return;
        }
        
        NextState();
    }

    private void NextState() {
        switch (State)
        {
            case SessionState.WaitStart:
                TimeLeft = CurrentQuestion.TimeLimit;
                State = SessionState.QuestionActive;
                break;
            case SessionState.QuestionActive:
                State = SessionState.QuestionEnd;
                break;
            case SessionState.QuestionEnd:
                if (Quiz.Questions.Count>QuestionIndex+1)
                {
                    QuestionIndex++;
                    TimeLeft = CurrentQuestion.TimeLimit;
                    State = SessionState.QuestionActive;
                }
                else {
                    State = SessionState.End;
                }
                break;
        }

        TimerEnabled = State == SessionState.QuestionActive;

        StateChanged?.Invoke(this, State);
    }

    public void StartQuiz() {
        if (State != SessionState.WaitStart)
        {
            return;
        }
        NextState();
    }

    public void NextQuestion() {
        if (State != SessionState.QuestionEnd)
        {
            return;
        }
        NextState();
    }

    public void EndQuestion() {
        if (State != SessionState.QuestionActive)
        {
            return;
        }
        NumberOfAnswersSent = 0;
        NextState();
    }

    private object ParticipantLock = new();

    public void AddParticipant(Participant p) {
        lock(ParticipantLock) {
            Participants.Add(p);
        }
    }

    private object NotificationLock = new();

    public void NotifySession(NotificationType nt) {
        lock (NotificationLock) {
            if (nt == NotificationType.AnswerSubmitted) {
                ++NumberOfAnswersSent;
            }
        }
    }
}

public enum NotificationType {
    AnswerSubmitted, UserLeftSession
}

public enum SessionState {
        WaitStart, QuestionActive, QuestionEnd, End
    }