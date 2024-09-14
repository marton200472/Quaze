using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace Quaze.Models;



public class Session
{
    public string Id { get; private set; }

    public string OwnerId { get; private set; }

    public ConcurrentBag<Participant> Participants { get; set; } = new();

    private int _numberOfAnswersSent = 0;
    public int NumberOfAnswersSent => _numberOfAnswersSent;
    public int TimeLeft { get; private set; } = 0;
    public int QuestionIndex { get; private set; } = 0;
    public SessionState State { get; private set; }

    public Question CurrentQuestion => Quiz.Questions[QuestionIndex];

    public event Action<SessionState> StateChanged;
    public event Action<int> TimerTick;
    public event Action ShouldRefreshInformation;

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
            TimerTick?.Invoke(TimeLeft);
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
                Interlocked.Exchange(ref _numberOfAnswersSent, 0);
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

        StateChanged?.Invoke(State);
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
        NextState();
    }


    public void AddParticipant(Participant p) {
        Participants.Add(p);
        ShouldRefreshInformation?.Invoke();
    }


    public void NotifySession(NotificationType nt) {
        if (nt == NotificationType.AnswerSubmitted) {
            Interlocked.Increment(ref _numberOfAnswersSent);
        }
        ShouldRefreshInformation?.Invoke();
    }
}

public enum NotificationType {
    AnswerSubmitted, UserLeftSession
}

public enum SessionState {
        WaitStart, QuestionActive, QuestionEnd, End
    }