namespace Quaze.Models;



public class Session
{
    public string Id { get; private set; }

    public List<Participant> Participants { get; set; } = new();


    public int TimeLeft { get; private set; } = 0;
    public int QuestionIndex { get; private set; } = 0;
    public State CurrentState { get; private set; }

    public event EventHandler StateChanged;
    public event EventHandler<int> TimerTick;

    public Session(string id, Quiz quiz) {
        Id = id;
        this.quiz = quiz;
    }

    private Quiz quiz;
    private bool TimerEnabled = false;

    public void OnTimerTick() {
        if (TimeLeft > 0)
        {
            TimeLeft--;
            TimerTick?.Invoke(this, TimeLeft);
            return;
        }
        else if(TimerEnabled)
            NextState();
    }

    private void NextState() {
        switch (CurrentState)
        {
            case State.WaitStart:
                TimeLeft = quiz.Questions[QuestionIndex].TimeLimit;
                TimerEnabled = true;
                CurrentState = State.QuestionActive;
                break;
            case State.QuestionActive:
                CurrentState = State.QuestionEnd;
                TimerEnabled = false;
                break;
            case State.QuestionEnd:
                if (quiz.Questions.Count>QuestionIndex)
                {
                    QuestionIndex++;
                    TimeLeft = quiz.Questions[QuestionIndex].TimeLimit;
                    CurrentState = State.QuestionActive;
                    TimerEnabled = true;
                }
                else {
                    CurrentState = State.End;
                    TimerEnabled = false;
                }
                break;
        }

        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public enum State {
        WaitStart, QuestionActive, QuestionEnd, End
    }
}