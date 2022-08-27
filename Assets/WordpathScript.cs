using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class WordpathScript : MonoBehaviour {

    public KMAudio Audio;
    public KMBombModule module;
    public KMBombInfo info;
    public List<KMSelectable> arrows;
    public KMSelectable submit;
    public GameObject hexgrid;
    public GameObject directarrow;
    public GameObject solvetick;
    public Renderer arrend;
    public Renderer[] leds;
    public Material[] travmats;
    public TextMesh[] letters;

    private readonly string alph = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y";
    private readonly string[,] words = new string[8, 8]
    {
        { "CAVITY", "IMPROV", "REDACT", "STUDIO", "MAGNET", "THUSLY", "WEBLOG", "LARYNX"},
        { "ECLAIR", "WHACKS", "PROVEN", "ATOMIC", "FRUGAL", "JASPER", "UTOPIA", "HUNGRY"},
        { "ADJOIN", "VORTEX", "QINTAR", "CRYPTS", "XENIAS", "OTHERS", "WYVERN", "OUTING"},
        { "NICKEL", "GLITCH", "KEYPAD", "INFLUX", "OBJECT", "EMBARK", "TORQUE", "SILVER"},
        { "QUOTAS", "PHYLUM", "NUMBER", "MOSAIC", "KNIVES", "SPHINX", "DETAIL", "BROWSE"},
        { "FORGET", "SQUAWK", "YACHTS", "DOUBLE", "JUMPER", "WIDGET", "CITRUS", "ANTHEM"},
        { "URCHIN", "RUSTIC", "BOXING", "HERMIT", "TRUDGE", "XYLOID", "LIMPET", "YONDER"},
        { "LAUNCH", "ITSELF", "JUNGLE", "VERIFY", "CHROME", "UPSIDE", "GNARLY", "KLAXON"}
    };
    private readonly int[,][] pgrid = new int[8, 8][]
    {
        { new int[5]{ 2, 10, 11, 21, 22}, new int[5]{ 10, 11, 18, 19, 29}, new int[5]{ 9, 11, 19, 20, 28}, new int[5]{ 1, 2, 10, 11, 20}, new int[4]{ 10, 11, 12, 22}, new int[4]{ 9, 17, 18, 28}, new int[5]{ 10, 11, 20, 21, 31}, new int[6]{ 1, 2, 10, 19, 20, 21} },
        { new int[6]{ 1, 2, 11, 18, 19, 20}, new int[5]{ 10, 11, 20, 21, 22}, new int[5]{ 10, 11, 19, 20, 30}, new int[6]{ 1, 8, 9, 10, 17, 18}, new int[7]{ 1, 9, 11, 19, 21, 29, 30}, new int[6]{ 10, 19, 20, 21, 22, 30}, new int[5]{ 9, 10, 18, 27, 28}, new int[5]{ 8, 10, 18, 19, 29} },
        { new int[5]{ 10, 20, 21, 22, 29}, new int[6]{ 9, 19, 20, 22, 30, 31}, new int[5]{ 9, 18, 28, 29, 30}, new int[4]{ 10, 11, 19, 28}, new int[6]{ 9, 10, 20, 21, 22, 31}, new int[5]{ 1, 2, 11, 12, 22}, new int[6]{ 2, 9, 10, 11, 12, 20}, new int[5]{ 1, 10, 19, 28, 29} },
        { new int[6]{ 1, 10, 11, 12, 21, 22}, new int[4]{ 9, 19, 29, 38}, new int[5]{ 8, 9, 18, 19, 27}, new int[5]{ 1, 9, 11, 19, 20}, new int[5]{ 9, 10, 20, 21, 30}, new int[6]{ 10, 18, 19, 20, 28, 38}, new int[6]{ 9, 10, 11, 18, 20, 29}, new int[5]{ 8, 9, 19, 20, 28} },
        { new int[6]{ 1, 2, 10, 18, 19, 20}, new int[6]{ 8, 9, 11, 19, 20, 30}, new int[5]{ 1, 2, 11, 12, 13}, new int[5]{ 9, 10, 17, 18, 27}, new int[5]{ 1, 8, 9, 19, 20}, new int[6]{ 9, 10, 19, 21, 22, 30}, new int[5]{ 1, 2, 9, 10, 18}, new int[5]{ 3, 10, 12, 20, 21} },
        { new int[6]{ 8, 10, 18, 19, 20, 28}, new int[4]{ 1, 11, 21, 22}, new int[5]{ 2, 10, 11, 19, 21}, new int[5]{ 1, 11, 12, 20, 22}, new int[6]{ 1, 10, 12, 20, 21, 22}, new int[4]{ 9, 10, 11, 20}, new int[6]{ 9, 10, 11, 21, 22, 31}, new int[6]{ 1, 9, 11, 12, 22, 31} },
        { new int[6]{ 9, 11, 18, 19, 20, 29}, new int[7]{ 1, 9, 11, 18, 20, 28, 29}, new int[6]{ 1, 10, 19, 20, 21, 30}, new int[6]{ 8, 9, 10, 11, 20, 30}, new int[4]{ 7, 8, 9, 16}, new int[5]{ 7, 8, 9, 19, 29}, new int[6]{ 9, 10, 19, 28, 29, 38}, new int[5]{ 8, 9, 16, 17, 18} },
        { new int[5]{ 9, 10, 11, 12, 21}, new int[6]{ 1, 2, 10, 12, 21, 22}, new int[5]{ 1, 10, 18, 19, 28}, new int[5]{ 8, 9, 17, 19, 28}, new int[5]{ 9, 10, 18, 19, 20}, new int[6]{ 9, 10, 18, 20, 28, 38}, new int[7]{ 1, 2, 9, 12, 19, 20, 21}, new int[5]{ 8, 9, 10, 11, 19} }
    };
    private string[,] lgrid = new string[9, 9]
    {
        { "-", "-", "-", "-", "-", "-", "-", "-", "-"},
        { "-", "", "", "", "", "-", "-", "-", "-",},
        { "-", "", "", "", "", "", "-", "-", "-",},
        { "-", "", "", "", "", "", "", "-", "-",},
        { "-", "", "", "", "", "", "", "", "-",},
        { "-", "-", "", "", "", "", "", "", "-",},
        { "-", "-", "-", "", "", "", "", "", "-",},
        { "-", "-", "-", "-", "", "", "", "", "-",},
        { "-", "-", "-", "-", "-", "-", "-", "-", "-"}
    };
    private int[] letterspots = new int[7] { -1, -1, -1, -1, -1, -1, -1};
    private int[] pathstart;
    private List<int> path = new List<int> { };
    private string pathlog;
    private int[] poly;
    private int state;
    private bool gridmove;
    private List<int> draw = new List<int> { };
    private int cell = 40;

    private static int moduleIDCounter;
    private int moduleID;
    private bool moduleSolved;

    private void Start()
    {
        moduleID = ++moduleIDCounter;
        arrend.enabled = false;
        int pickword = Random.Range(0, 64);
        string word = words[pickword / 8, pickword % 8];
        for (int i = 0; i < 7; i++)
        {
            int p = RandHex();
            while (letterspots.Contains(p))
                p = RandHex();
            letterspots[i] = p;
            lgrid[p / 9, p % 9] = i == 6 ? "Z" : word[i].ToString();
            letters[p].text = lgrid[p / 9, p % 9];
        }
        pathlog = "[" + word[0].ToString() + "]";
        for (int i = 1; i < 8; i++)
            for (int j = Mathf.Max(1, i - 3); j < Mathf.Min(i + 4, 8); j++)
            {
                if (lgrid[i, j] == "")
                {
                    List<string> plaus = alph.Split(',').ToList();
                    plaus.Remove(lgrid[i, j - 1]);
                    plaus.Remove(lgrid[i - 1, j - 1]);
                    plaus.Remove(lgrid[i - 1, j]);
                    plaus.Remove(lgrid[i, j + 1]);
                    plaus.Remove(lgrid[i + 1, j + 1]);
                    plaus.Remove(lgrid[i + 1, j]);
                    lgrid[i, j] = plaus.PickRandom();
                }
                letters[i * 9 + j].text = lgrid[i, j];
            }
        for (int i = 0; i < 5; i++)
        {
            pathstart = new int[2] { letterspots[i] / 9, letterspots[i] % 9 };
            Debug.Log("> " + string.Join(", ", pathstart.Select(x => x.ToString()).ToArray()));
            int[] offset = new int[2] { pathstart[0] - (letterspots[i + 1] / 9), pathstart[1] - (letterspots[i + 1] % 9) };
            Debug.Log(offset[0] + ", " + offset[1]);
            List<int> subpath = new List<int> { };
            subpath = Subpath(subpath, offset);
            Debug.Log("[] " + string.Join(", ", subpath.Select(x => x.ToString()).ToArray()));
            while (Random.Range(3, 7) >= subpath.Count())
                Noise(subpath);
            path = path.Concat(subpath).ToList();
            int p = path.Last() + 10;
            path.RemoveAt(path.Count() - 1);
            path.Add(p);
            Debug.Log("[][] " + string.Join(", ", subpath.Select(x => x.ToString()).ToArray()));
            for (int j = 0; j < subpath.Count() - 1; j++)
            {
                Step(pathstart, subpath[j]);
                Debug.Log(">> " + string.Join(", ", pathstart.Select(x => x.ToString()).ToArray()));
                pathlog += lgrid[pathstart[0], pathstart[1]];
            }
            Step(pathstart, subpath.Last());
            pathlog += "[" + lgrid[pathstart[0], pathstart[1]] + "]";
        }
        Debug.LogFormat("[Wordpath #{0}] The arrow flashes the sequence of bearings: {1}", moduleID, string.Join(", ", path.Select(x => new string[] { "180", "240", "300", "000", "060", "120"}[x % 10]).ToArray()));
        Debug.LogFormat("[Wordpath #{0}] The path traverses the cells in the order: {1}", moduleID, pathlog);
        Debug.LogFormat("[Wordpath #{0}] The letters of the word lie in the cells: {1}", moduleID, string.Join(", ", letterspots.Take(6).Select(x => HexPos(x)).ToArray()));
        Debug.LogFormat("[Wordpath #{0}] The Z cell is located at {1}.", moduleID, HexPos(letterspots[6]));
        int[] g = new int[2] { pickword / 8, (pickword % 8) * 2 };
        if (g[0] % 2 == 0) g[1]++;
        int dir = 0;
        if (info.GetIndicators().Count() == 0 || string.Join("", info.GetIndicators().ToArray()).Any(x => !pathlog.Contains(x.ToString())))
        { 
            if (pathlog.Contains("Z"))
                dir = 4;
            else if (pathlog.Where(x => x != '[' && x != ']' && !word.Contains(x.ToString())).GroupBy(x => x).Any(z => z.Count() > 3))
                dir = 8;
            else
                dir = 12;
        }
        if(info.GetSerialNumberLetters().Any(x => pathlog.Contains(x)))
        {
            if (letterspots.Take(6).Any(x => new string[6] { lgrid[x / 9 - 1, x % 9 - 1], lgrid[x / 9 - 1, x % 9], lgrid[x / 9, x % 9 - 1], lgrid[x / 9, x % 9 + 1], lgrid[x / 9 + 1, x % 9], lgrid[x / 9 + 1, x % 9 + 1] }.Contains("Z")))
                dir++;
            else if (new int[] { 10, 11, 12, 13, 19, 23, 28, 33, 37, 43, 47, 52, 57, 61, 67, 68, 69, 70 }.Contains(letterspots[0]))
                dir += 2;
            else
                dir += 3;
        }
        Debug.LogFormat("[Wordpath #{0}] The {1} row and {2} column rules apply.", moduleID, new string[] { "first", "second", "third", "fourth"}[dir / 4], new string[] { "first", "second", "third", "fourth"}[dir % 4]);
        dir = new int[16] { 0, 1, 2, 3, 1, 0, 3, 2, 2, 3, 0, 1, 3, 2, 1, 0}[dir];
        Debug.LogFormat("[Wordpath #{0}] The target polyhex is one space {1} {2}.", moduleID, new string[] { "above", "to the right of", "below", "to the left of"}[dir], word);
        switch (dir)
        {
            case 0: g[0] = (g[0] + 7) % 8; break;
            case 1: g[1] = (g[1] + 1) % 16; break;
            case 2: g[0] = (g[0] + 1) % 8; break;
            default: g[1] = (g[1] + 15) % 16; break;
        }
        if (g[0] % 2 == 1)
            g[1]--;
        g[1] /= 2;
        poly = pgrid[g[0], g[1]];
        Debug.Log("(" + g[0] + "," + g[1] + ") - " + string.Join(", ", poly.Select(x => x.ToString()).ToArray()));
        foreach(KMSelectable arrow in arrows)
        {
            int a = arrows.IndexOf(arrow);
            arrow.OnInteract = delegate ()
            {
                if(state > 1 && !gridmove)
                {                    
                    int pcell = cell + new int[6] { -9, 1, 10, 9, -1, -10}[a];
                    if (lgrid[pcell / 9, pcell % 9] != "-")
                    {
                        arrow.AddInteractionPunch(0.1f);
                        leds[cell].material = travmats[state > 2 ? 1 : 2];
                        cell = pcell;
                        leds[cell].material = travmats[0];
                        if(state > 2)
                        {
                            if (!draw.Contains(cell))
                            {
                                draw.Add(cell);
                                Audio.PlaySoundAtTransform("Draw", arrow.transform);
                            }
                            else
                                Audio.PlaySoundAtTransform("Move", arrow.transform);
                        }
                        else
                            Audio.PlaySoundAtTransform("Move", arrow.transform);
                    }
                }
                return false;
            };
        }
        submit.OnInteract = delegate ()
        {
            if (!moduleSolved && !gridmove)
            {
                submit.AddInteractionPunch(0.5f);
                Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, submit.transform);
                switch (state)
                {
                    case 0:
                        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.WireSequenceMechanism, transform);
                        StartCoroutine(Movegrid(true));
                        break;
                    case 1:
                        StopCoroutine("ArrSeq");
                        arrend.enabled = false;
                        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.WireSequenceMechanism, transform);
                        StartCoroutine(Movegrid(false));
                        foreach (TextMesh l in letters)
                            if(l != null)
                                l.text = "";
                        cell = 40;
                        leds[40].material = travmats[0];
                        break;
                    case 3:
                        int[] submission = draw.OrderBy(x => x).ToArray();
                        Debug.LogFormat("[Wordpath #{0}] Submitted the region consisting of the cells: {1}", moduleID, string.Join(", ", draw.Select(x => HexPos(x)).ToArray()));
                        int[] shape = submission.Select(x => x - submission[0]).Skip(1).ToArray();
                        if (shape.SequenceEqual(poly))
                        {
                            Debug.LogFormat("[Wordpath #{0}] 1: The submitted region does not match the target polyhex.", moduleID);
                            int[] p = draw.Intersect(letterspots.Take(6)).ToArray();
                            if(p.Length == 1)
                            {
                                Debug.LogFormat("[Wordpath #{0}] 2: The submitted region contains one cell from the word: {1}", moduleID, HexPos(p[0]));
                                if(p[0] == cell)
                                {
                                    Debug.LogFormat("[Wordpath #{0}] 3: {1} is highlighted in white.", moduleID, HexPos(p[0]));
                                    moduleSolved = true;
                                    state = -1;
                                    module.HandlePass();
                                    Audio.PlaySoundAtTransform("Solve", transform);
                                    solvetick.SetActive(false);
                                    StartCoroutine(Movegrid(true));
                                }
                                else
                                {
                                    Debug.LogFormat("[Wordpath #{0}] {1} is highlighted in white.", moduleID, HexPos(cell));
                                    Strikereset();
                                }
                            }
                            else
                            {
                                Debug.LogFormat("[Wordpath #{0}] The submitted region contains {1} cells from the word{2}", moduleID, p.Length, p.Length < 1 ? "." : (": " + string.Join(", ", p.Select(x => HexPos(x)).ToArray())));
                                Strikereset();
                            }
                        }
                        else
                        {
                            Debug.LogFormat("[Wordpath #{0}] The submitted region does not match the target polyhex.", moduleID);
                            Debug.Log(string.Join(", ", shape.Select(x => x.ToString()).ToArray()));
                            Strikereset();
                        }
                        break;
                    default:
                        state++;
                        draw.Add(cell);
                        break;
                }
            }
            return false;
        };
    }

    private int RandHex()
    {
        int y = Random.Range(1, 8);
        int x = Random.Range(Mathf.Max(1, y - 3), Mathf.Min(y + 4, 8));
        return y * 9 + x;
    }

    private string HexPos(int c)
    {
        string x = "ABCDEFG"[(c % 9) - 1].ToString();
        int y = (c / 9) - Mathf.Max(0, (c % 9) - 4); 
        return x + y.ToString();
    }

    private List<int> Subpath(List<int> s, int[] o)
    {
        if (o[0] == 0)
        {
            if (o[1] > 0)
                for (int i = 0; i < o[1]; i++)
                    s.Add(1);
            else
                for (int i = 0; i > o[1]; i--)
                    s.Add(4);
        }
        else if (o[1] == 0)
        {
            if (o[0] > 0)
                for (int i = 0; i < o[0]; i++)
                    s.Add(3);
            else
                for (int i = 0; i > o[0]; i--)
                    s.Add(0);
        }
        else if (o[0] == o[1])
        {
            if (o[0] > 0)
                for (int i = 0; i < o[0]; i++)
                    s.Add(2);
            else
                for (int i = 0; i > o[0]; i--)
                    s.Add(5);
        }
        else
        {
            if (o[0] * o[1] < 0)
            {
                if (o[0] > o[1])
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        s.Add(3);
                        s.Concat(Subpath(s, new int[] { o[0] - 1, o[1]}));
                    }
                    else
                    {
                        s.Add(4);
                        s.Concat(Subpath(s, new int[] { o[0], o[1] + 1}));
                    }
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        s.Add(0);
                        s.Concat(Subpath(s, new int[] { o[0] + 1, o[1]}));
                    }
                    else
                    {
                        s.Add(1);
                        s.Concat(Subpath(s, new int[] { o[0], o[1] - 1}));
                    }
                }
            }
            else if(o[0] > 0)
            {
                if (o[0] > o[1])
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        s.Add(3);
                        s.Concat(Subpath(s, new int[] { o[0] - 1, o[1]}));
                    }
                    else
                    {
                        s.Add(2);
                        s.Concat(Subpath(s, new int[] { o[0] - 1, o[1] - 1}));
                    }
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        s.Add(1);
                        s.Concat(Subpath(s, new int[] { o[0], o[1] - 1 }));
                    }
                    else
                    {
                        s.Add(2);
                        s.Concat(Subpath(s, new int[] { o[0] - 1, o[1] - 1}));
                    }
                }
            }
            else
            {
                if (o[0] > o[1])
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        s.Add(5);
                        s.Concat(Subpath(s, new int[] { o[0] + 1, o[1] + 1 }));
                    }
                    else
                    {
                        s.Add(4);
                        s.Concat(Subpath(s, new int[] { o[0], o[1] + 1}));
                    }
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        s.Add(0);
                        s.Concat(Subpath(s, new int[] { o[0] + 1, o[1] }));
                    }
                    else
                    {
                        s.Add(5);
                        s.Concat(Subpath(s, new int[] { o[0] + 1, o[1] + 1 }));
                    }
                }
            }
        }
        return s;
    }

    private List<int> Noise(List<int> s)
    {
        int p = s.PickRandom();
        s.Remove(p);
        s.Add((p + 1) % 6);
        s.Add((p + 5) % 6);
        reshuffle:;
            s = s.Shuffle();
        for (int i = 0; i < s.Count() - 1; i++)
            if (Mathf.Abs(s[i + 1] - s[i]) == 3)
                goto reshuffle;
        int[] ps = new int[2] { pathstart[0], pathstart[1] };
        for (int i = 0; i < s.Count(); i++)
        {
            ps = Step(ps, s[i]);
            if (lgrid[ps[0], ps[1]] == "-")
                goto reshuffle;
        }
        return s;
    }

    private int[] Step(int[] p, int m)
    {
        switch (m)
        {
            case 0: p[0]++; break;
            case 1: p[1]--; break;
            case 2: p[0]--; p[1]--; break;
            case 3: p[0]--; break;
            case 4: p[1]++; break;
            default: p[0]++; p[1]++; break;
        }
        return p;
    }

    private IEnumerator Movegrid(bool s)
    {
        gridmove = true;       
        float v = -0.01f;
        if (s)
            v *= -1;
        for (int i = 0; i < 60; i++)
        {
            hexgrid.transform.localPosition += new Vector3(0, 0, v);
            if (moduleSolved)
                solvetick.transform.localPosition -= new Vector3(0, 0, v);
            else
                directarrow.transform.localPosition -= new Vector3(0, 0, v);
            yield return new WaitForSeconds(0.03f);
        }
        state++;
        if (state == 1)
            StartCoroutine("ArrSeq");
        gridmove = false;
        if (moduleSolved)
        {
            yield return new WaitForSeconds(1.2f);
            solvetick.SetActive(true);
        }
    }

    private IEnumerator ArrSeq()
    {
        while (!moduleSolved)
        {
            for(int i = 0; i < path.Count(); i++)
            {
                float v = new int[] {270, 210, 150, 90, 30, 330}[path[i] % 10];
                directarrow.transform.localRotation = Quaternion.Euler(0, 0, v);
                arrend.enabled = true;
                arrend.material = travmats[path[i] >= 10 ? 1 : 0];
                yield return new WaitForSeconds(0.75f);
                arrend.enabled = false;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void Strikereset()
    {
        draw.Clear();
        module.HandleStrike();
        state = 0;
        foreach (Renderer l in leds)
            if (l != null)
                l.material = travmats[2];
        for (int i = 1; i < 8; i++)
            for (int j = Mathf.Max(1, i - 3); j < Mathf.Min(i + 4, 8); j++)
                letters[i * 9 + j].text = lgrid[i, j];
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = "!{0} <0/2/4/6/8/10> [Movements as clockface. Chain with spaces.] !{0} next/submit [Presses hexagonal button]";
#pragma warning restore 414

    private readonly List<string> arr = new List<string> { "0", "2", "4", "6", "8", "10" };

    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        if(command == "next")
        {
            if(state > 2)
            {
                yield return "sendtochaterror!f Use \"submit\" to submit the highlighted region.";
                yield break;
            }
            else
            {
                yield return null;
                submit.OnInteract();
            }
        }
        else if (command == "submit")
        {
            if (state > 2)
            {
                yield return null;
                submit.OnInteract();
            }
            else
            {
                yield return "sendtochaterror!f Use \"next\" to move to the next phase of the module.";
                yield break;
            }
        }
        else
        {
            if(state < 2)
            {
                yield return "sendtochaterror!f Arrows cannot be pressed at this phase of the module.";
                yield break;
            }
            List<string> dir = command.Split(' ').ToList();
            dir.RemoveAll(x => x == "" || x == " ");
            List<int> p = new List<int> { };
            for(int i = 0; i < dir.Count(); i++)
            {
                int d = arr.IndexOf(dir[i]);
                if(d < 0)
                {
                    yield return "sendtochaterror!f " + dir[i] + " is not a valid arrow.";
                    yield break;
                }
                p.Add(d);
            }
            for(int i = 0; i < p.Count(); i++)
            {
                yield return null;
                arrows[p[i]].OnInteract();
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
