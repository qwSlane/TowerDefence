using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[System.Serializable]
public struct EnemyAnimator
{
    public enum Clip { Move };

    PlayableGraph graph;

    public Clip CurrentClip { get; private set; }

    public bool IsDone => GetPlayable(CurrentClip).IsDone();

    AnimationMixerPlayable mixer;

    public void Configure(Animator animator, EnemyAnimationConfig config) {
        graph = PlayableGraph.Create();
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        mixer = AnimationMixerPlayable.Create(graph, 1);

        var clip = AnimationClipPlayable.Create(graph, config.Move);
        clip.Pause();
        mixer.ConnectInput((int)Clip.Move, clip, 0);

        var output = AnimationPlayableOutput.Create(graph, "Enemy", animator);
        output.SetSourcePlayable(clip);
    }

    public void PlayMove(float speed) {
        graph.GetOutput(0).GetSourcePlayable().SetSpeed(speed);
        graph.Play();
        CurrentClip = Clip.Move;
    }

    Playable GetPlayable(Clip clip) {
        return mixer.GetInput((int)clip);
    }

    public void Stop() {
        graph.Stop();
    }

    void SetWeight(Clip clip, float weight) {
        mixer.SetInputWeight((int)clip, weight);
    }

    public void Destroy() {
        graph.Destroy();
    }
}
