using SolarStorms.Gameplay;
using SolarStorms.Simulation;
using UnityEngine;

namespace SolarStorms.Presentation
{
    /// <summary>
    /// Placeholder reactor sounds, driven only by reactor events: a distinct short tick per system on every
    /// routing change, one tick for a preset, a looping alarm during an overheat lockout, and a release tone after it.
    /// </summary>
    public sealed class ReactorAudio : MonoBehaviour
    {
        [SerializeField] ShipReactor reactor;
        [SerializeField] ReactorFeedbackData feedback;

        readonly AudioClip[] systemTicks = new AudioClip[ReactorModel.SystemCount];
        AudioClip presetTick;
        AudioClip releaseTone;
        AudioSource tickSource;
        AudioSource alarmSource;

        void Awake()
        {
            tickSource = gameObject.AddComponent<AudioSource>();
            tickSource.playOnAwake = false;

            alarmSource = gameObject.AddComponent<AudioSource>();
            alarmSource.playOnAwake = false;
            alarmSource.loop = true;
            alarmSource.clip = PlaceholderTones.Alarm("Overheat Alarm", feedback.overheatAlarm, feedback.volume);

            for (int i = 0; i < systemTicks.Length; i++)
            {
                var system = (ReactorSystem)i;
                systemTicks[i] = PlaceholderTones.Beep($"{system} Tick", feedback.TickFor(system), feedback.tickSeconds, feedback.volume);
            }

            presetTick = PlaceholderTones.Beep("Preset Tick", feedback.presetTick, feedback.tickSeconds * 2f, feedback.volume);
            releaseTone = PlaceholderTones.Beep("Overheat Release", feedback.overheatRelease, feedback.tickSeconds * 3f, feedback.volume);
        }

        void OnEnable()
        {
            ReactorModel model = reactor.Model;
            model.PowerChanged += OnPowerChanged;
            model.OverheatStarted += OnOverheatStarted;
            model.OverheatEnded += OnOverheatEnded;
        }

        void OnDisable()
        {
            ReactorModel model = reactor.Model;
            model.PowerChanged -= OnPowerChanged;
            model.OverheatStarted -= OnOverheatStarted;
            model.OverheatEnded -= OnOverheatEnded;
        }

        void OnPowerChanged(PowerChange change)
        {
            tickSource.PlayOneShot(change.IsPreset ? presetTick : systemTicks[(int)change.System]);
        }

        void OnOverheatStarted() => alarmSource.Play();

        void OnOverheatEnded()
        {
            alarmSource.Stop();
            tickSource.PlayOneShot(releaseTone);
        }
    }
}
