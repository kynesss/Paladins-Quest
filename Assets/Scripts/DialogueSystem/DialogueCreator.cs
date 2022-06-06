using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCreator : MonoBehaviour
{
    private static DialogueCreator instance;

    public static DialogueCreator MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<DialogueCreator>();
            }
            return instance;
        }
    }

    public bool dialogueAdded = false;

    public void ClearListsAndSetDialogues(NPC npc)
    {
        ClearDialogueLists(npc);

        AddTemporaryDialogues(npc);
    }
    private void AddTemporaryDialogues(NPC npc)
    {
        if (npc._name == "Mark")
        {
            //Quest dialogi
            npc.playerQuestDialogue.Add("Witaj masz dla mnie jakie� zadanie?");
            npc.npcQuestDialogue.Add("Witam ci� podr�niku! Od dw�ch dni nic nie jad�em czy m�g�by� �askawie przynie�� mi jeden bochenek chleba?");
            npc.playerQuestDialogue.Add("Jasne ju� si� robi!");
            npc.playerQuestDialogue.Add("Mo�e nast�pnym razem! Mam teraz inne sprawy na g�owie.");
            npc.npcQuestDialogue.Add("Dzi�ki i do zobaczenia!");
            npc.npcQuestDialogue.Add("Rozumiem! Do zobaczenia!");

            //Talkdialogi
            npc.playerTalkDialogue.Add("Co s�ycha� w mie�cie?");
            npc.npcTalkDialogue.Add("Nic nowego!");
            npc.playerTalkDialogue.Add("A to �egnaj!");
            npc.npcTalkDialogue.Add("Do zobaczenia!");
        }
        else if(npc._name == "John")
        {
            //Quest dialogi
            npc.playerQuestDialogue.Add("Hej dlaczego jeste� taki przera�ony?");
            npc.npcQuestDialogue.Add("Witaj! Po prostu boj� si� tych przekl�tych szkielet�w! Ma�o tego jeden siedzi w�a�nie w mojej chacie!");
            npc.playerQuestDialogue.Add("Nie martw si�, zajm� si� tym!");
            npc.playerQuestDialogue.Add("Jeste� taki stary a boisz si� szkielet�w! Rad� sobie sam!");
            npc.npcQuestDialogue.Add("Dzi�kuj� bardzo, oczywi�cie dam ci co� za to!");
            npc.npcQuestDialogue.Add("�egnaj!");
        }
        else if (npc._name == "Martin")
        {
            //Talk dialogi
            npc.playerTalkDialogue.Add("Witaj czego tutaj pilnujesz?");
            npc.npcTalkDialogue.Add("Strzeg� tego przej�cia za mn�, aby nikt pod �adnym pozorem tam nie wchodzi�!");
            npc.playerTalkDialogue.Add("Dlaczego nikt nie mo�e tam wej��?");
            npc.npcTalkDialogue.Add("Ca�a jaskinia a� roi si� od umarlak�w i innych niebezpiecznych kreatur!");

            //Quest dialogi
            npc.playerQuestDialogue.Add("Chcia�bym wej�� do jaskini, poradz� sobie z potworami!");
            npc.npcQuestDialogue.Add("Ha ha ha! Nie ma takiej opcji, samego ci� tam nie wpuszcz� zginiesz i b�d� ci� mia� na sumieniu!");
            npc.playerQuestDialogue.Add("Okej rozumiem. To w takim razie chod� ze mn�? Chyba taki wielki ch�op jak ty si� nie boi?");
            npc.playerQuestDialogue.Add("W takim razi� b�d� musia� ci� zabi�!");
            npc.npcQuestDialogue.Add("Ja si� boj�?! Ja si� niczego nie boj�! Dawaj ruszajmy oczy��my t� jaskini� raz na zawsze!");
            npc.npcQuestDialogue.Add("Hahah tylko spr�buj mi�czaku!");
        }
        else if (npc._name == "Bulit")
        {
            //Talk dialogi
            npc.playerTalkDialogue.Add("Co tutaj porabiasz?");
            npc.npcTalkDialogue.Add("Nie mam nic sensownego do roboty! Stoj� i patrz� na morze. Zauwa�y�em ci� ju� z oddali jak do nas przyp�yn��e�! Czego szukasz w naszym mie�cie?");
            npc.playerTalkDialogue.Add("Chcia�bym zarobi� troch� z�ota i kupi� solidny ekwipunek, dlatego chodz� po mie�cie i rozmawiam z lud�mi.");
            npc.npcTalkDialogue.Add("W dzisiejszych czasach z�ota nigdy ma�o. Porozmawiaj z kilkoma lud�mi na pewno co� si� znajdzie do roboty.");

            //Quest dialogi
            npc.playerQuestDialogue.Add("Nie potrzebujesz czasem czyjej� pomocy? Potrzebuj� zarobi� troch� z�ota!");
            npc.npcQuestDialogue.Add("Oczywi�cie �e potrzebuj� z nieba mi spad�e�! Dooko�a pla� a� roi si� od niebezpiecznych krab�w! Sam na w�asne oczy widzia�em jak jeden z nich po�era cz�owieka!" +
                "Kto� musi w ko�cu zrobi� z nimi porz�dek! Piszesz si�? Oczywi�cie dostaniesz za to 2000 sztuk z�ota!");
            npc.playerQuestDialogue.Add("2000 sztuk z�ota! To masa pi�ni�dzy! B�dzie to nie lada wyzwanie, ale dam rad�!");
            npc.playerQuestDialogue.Add("Nie ma mowy! Chcesz �eby mnie te kraby po�ar�y �ywcem?!");
            npc.npcQuestDialogue.Add("Fantastycznie! Powodzenia przyjacielu!");
            npc.npcQuestDialogue.Add("Ehh co z ciebie za wojownik? Spadaj!");
        }
        else if (npc._name == "Bandyta")
        {
            //Talk dialogi
            npc.npcQuestDialogue.Add("Zatrzymaj si� w tej chwili! Dalej nie przejdziesz, chyba �e zap�acisz mi 3000 sztuk z�ota!");

            npc.playerQuestDialogue.Add("Chyba �nisz! Lepiej zejd� mi z drogi!");

            npc.npcQuestDialogue.Add("Powtarzam jeszcze raz! Albo 3000 z�ota albo gorzko tego po�a�ujesz!");

            npc.playerQuestDialogue.Add("Trzymaj te swoje 3000 i zostaw mnie w spokoju!"); 
            npc.playerQuestDialogue.Add("Nic ode mnie nie dostaniesz z�odzieju!"); 

            npc.npcQuestDialogue.Add("Noo i to mi si� podoba! Prosz� bardzo mo�esz i�� dalej!");
            npc.npcQuestDialogue.Add("Ajj i widzisz! Mia�em dla ciebie grzeczn� propozycj�! Sam tego chcia�e�!");


        }
    }
    public void AddQuestRewardDialogues(NPC npc)
    {
        if (npc._name == "John")
        {
            ClearDialogueLists(npc);

            npc.playerTalkDialogue.Add("Mo�esz ju� wr�ci� bezpiecznie do domu!");
            npc.npcTalkDialogue.Add("Nie masz poj�cia jak bardzo jestem Ci wdzi�czny w�drowcze!");
            npc.playerTalkDialogue.Add("Starczy tej wdzi�czno�ci! Dawaj nagrod�!");
            npc.npcTalkDialogue.Add("Oczywi�cie! My�l� �e b�dziesz zadowolony trzymaj!");
            dialogueAdded = true;
        }
        if(npc._name == "Mark")
        {
            ClearDialogueLists(npc);

            npc.playerTalkDialogue.Add("Prosz� oto tw�j chleb!");
            npc.npcTalkDialogue.Add("Ooo brawo! Wiedzia�em �e dasz sobie rad�.");
            npc.playerTalkDialogue.Add("Co z moj� nagrod�?");
            npc.npcTalkDialogue.Add("Ale� prosz� ci� bardzo! Trzymaj!");
            dialogueAdded = true;
        }
        if(npc._name == "Martin")
        {
            ClearDialogueLists(npc);

            npc.playerTalkDialogue.Add("Noo g�adko posz�o przyjacielu!");
            npc.npcTalkDialogue.Add("Nie ma silnych na dw�ch paladyn�w!");
            npc.playerTalkDialogue.Add("Mia�e� racj� bez twojej pomocy nie da�bym rady.");
            npc.npcTalkDialogue.Add("Nie martw si� wojowniku! Nawet mi by�oby cie�ko walczy� z tymi potworami w pojedynk�! Nie zawiod�em si� na tobie, trzymaj za to nagrod�!");
            dialogueAdded = true;
        }
        if (npc._name == "Bulit")
        {
            ClearDialogueLists(npc);

            npc.playerTalkDialogue.Add("Wielkie kraby ju� wi�cej nie b�d� niepokoi� ludzi! Wszystkie w�chaj� kwiatki od spodu!");
            npc.npcTalkDialogue.Add("Niesamowite! A jednak uda�o ci si�! Brawo!");
            npc.playerTalkDialogue.Add("No uda� mi si� uda�o, ale by�o to nie lada wyzwanie! To co z moim z�otem?");
            npc.npcTalkDialogue.Add("Oczywi�cie! Ja zawsze dotrzymuj� s�owa. Oto twoje 2000 z�ota!");
            dialogueAdded = true;
        }
    }

    private void ClearDialogueLists(NPC npc)
    {
        npc.npcQuestDialogue.Clear();
        npc.playerQuestDialogue.Clear();
        npc.npcTalkDialogue.Clear();
        npc.playerTalkDialogue.Clear();
    }
}
