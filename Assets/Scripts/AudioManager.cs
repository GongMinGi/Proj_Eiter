using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; //1. �����޸𸮿� ������� instance�� ������ ����

    [Header("#BGM")] //3.
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")] //3.
    [Header("#SFX")] //3.
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;//4. ä�� ���� ���� ���� for �ٷ��� ȿ������
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { attack3, attacksuccess, Cat,ChargeattackCharging, ChargeAttackRelease1_1, dash1, Falling, glide, Iamattattacked, jump1,Mosquito,RockCrush,Snail,SpiderNo4,Zombie2}

    void Awake() //5. ä�� �ε��� ����-ä���� �� ���ΰ�?
    {
        instance = this; //2
        Init();
    }

    void Init()
    {

        //6. ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false; //7. ó�� Ű�ڸ��� ������ �����
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //ȿ���� �÷��̾� �ʱ�ȭ+
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
//������� �ϳ� , ȿ������ ä���� ������



// Fin ����ٰ� ���ϴ� ���� �޴���? ���ϵ鿡 ������ �ڵ带 �ֱ�

//Fin1 �켱,  ȿ������ ��� gamemanager ���� script�� �޼ҵ�? ���ٰ�
//AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);//���⼭ Select�� �����̸��̴� //24�� ��ó�� ����
//Fin1-1 ���鰰�� ���� ��� ����� ���� ���� �ÿ��� ���� �ʵ��� ���� �߰��ϱ�
//Fin1-2 �ΰ� �̻� �Ҹ� ���� ������ ����

//Fin2. AudioManager.instance.PlayBgm(true); ���ۺαٿ��ٰ�
//Fin2. AudioManager.instance.PlayBgm(false);������ �αٿ��ٰ�
//Fin2-1. highpass filter�̷��ų� ui������ bgm���߰ų� �׷��°� �ϴ� pass //33�� �α�?
