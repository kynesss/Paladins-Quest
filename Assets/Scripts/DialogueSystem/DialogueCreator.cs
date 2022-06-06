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
            npc.playerQuestDialogue.Add("Witaj masz dla mnie jakieœ zadanie?");
            npc.npcQuestDialogue.Add("Witam ciê podró¿niku! Od dwóch dni nic nie jad³em czy móg³byœ ³askawie przynieœæ mi jeden bochenek chleba?");
            npc.playerQuestDialogue.Add("Jasne ju¿ siê robi!");
            npc.playerQuestDialogue.Add("Mo¿e nastêpnym razem! Mam teraz inne sprawy na g³owie.");
            npc.npcQuestDialogue.Add("Dziêki i do zobaczenia!");
            npc.npcQuestDialogue.Add("Rozumiem! Do zobaczenia!");

            //Talkdialogi
            npc.playerTalkDialogue.Add("Co s³ychaæ w mieœcie?");
            npc.npcTalkDialogue.Add("Nic nowego!");
            npc.playerTalkDialogue.Add("A to ¿egnaj!");
            npc.npcTalkDialogue.Add("Do zobaczenia!");
        }
        else if(npc._name == "John")
        {
            //Quest dialogi
            npc.playerQuestDialogue.Add("Hej dlaczego jesteœ taki przera¿ony?");
            npc.npcQuestDialogue.Add("Witaj! Po prostu bojê siê tych przeklêtych szkieletów! Ma³o tego jeden siedzi w³aœnie w mojej chacie!");
            npc.playerQuestDialogue.Add("Nie martw siê, zajmê siê tym!");
            npc.playerQuestDialogue.Add("Jesteœ taki stary a boisz siê szkieletów! RadŸ sobie sam!");
            npc.npcQuestDialogue.Add("Dziêkujê bardzo, oczywiœcie dam ci coœ za to!");
            npc.npcQuestDialogue.Add("¯egnaj!");
        }
        else if (npc._name == "Martin")
        {
            //Talk dialogi
            npc.playerTalkDialogue.Add("Witaj czego tutaj pilnujesz?");
            npc.npcTalkDialogue.Add("Strzegê tego przejœcia za mn¹, aby nikt pod ¿adnym pozorem tam nie wchodzi³!");
            npc.playerTalkDialogue.Add("Dlaczego nikt nie mo¿e tam wejœæ?");
            npc.npcTalkDialogue.Add("Ca³a jaskinia a¿ roi siê od umarlaków i innych niebezpiecznych kreatur!");

            //Quest dialogi
            npc.playerQuestDialogue.Add("Chcia³bym wejœæ do jaskini, poradzê sobie z potworami!");
            npc.npcQuestDialogue.Add("Ha ha ha! Nie ma takiej opcji, samego ciê tam nie wpuszczê zginiesz i bêdê ciê mia³ na sumieniu!");
            npc.playerQuestDialogue.Add("Okej rozumiem. To w takim razie chodŸ ze mn¹? Chyba taki wielki ch³op jak ty siê nie boi?");
            npc.playerQuestDialogue.Add("W takim raziê bêdê musia³ ciê zabiæ!");
            npc.npcQuestDialogue.Add("Ja siê bojê?! Ja siê niczego nie bojê! Dawaj ruszajmy oczyœæmy t¹ jaskiniê raz na zawsze!");
            npc.npcQuestDialogue.Add("Hahah tylko spróbuj miêczaku!");
        }
        else if (npc._name == "Bulit")
        {
            //Talk dialogi
            npc.playerTalkDialogue.Add("Co tutaj porabiasz?");
            npc.npcTalkDialogue.Add("Nie mam nic sensownego do roboty! Stojê i patrzê na morze. Zauwa¿y³em ciê ju¿ z oddali jak do nas przyp³yn¹³eœ! Czego szukasz w naszym mieœcie?");
            npc.playerTalkDialogue.Add("Chcia³bym zarobiæ trochê z³ota i kupiæ solidny ekwipunek, dlatego chodzê po mieœcie i rozmawiam z ludŸmi.");
            npc.npcTalkDialogue.Add("W dzisiejszych czasach z³ota nigdy ma³o. Porozmawiaj z kilkoma ludŸmi na pewno coœ siê znajdzie do roboty.");

            //Quest dialogi
            npc.playerQuestDialogue.Add("Nie potrzebujesz czasem czyjejœ pomocy? Potrzebujê zarobiæ trochê z³ota!");
            npc.npcQuestDialogue.Add("Oczywiœcie ¿e potrzebujê z nieba mi spad³eœ! Dooko³a pla¿ a¿ roi siê od niebezpiecznych krabów! Sam na w³asne oczy widzia³em jak jeden z nich po¿era cz³owieka!" +
                "Ktoœ musi w koñcu zrobiæ z nimi porz¹dek! Piszesz siê? Oczywiœcie dostaniesz za to 2000 sztuk z³ota!");
            npc.playerQuestDialogue.Add("2000 sztuk z³ota! To masa piêniêdzy! Bêdzie to nie lada wyzwanie, ale dam radê!");
            npc.playerQuestDialogue.Add("Nie ma mowy! Chcesz ¿eby mnie te kraby po¿ar³y ¿ywcem?!");
            npc.npcQuestDialogue.Add("Fantastycznie! Powodzenia przyjacielu!");
            npc.npcQuestDialogue.Add("Ehh co z ciebie za wojownik? Spadaj!");
        }
        else if (npc._name == "Bandyta")
        {
            //Talk dialogi
            npc.npcQuestDialogue.Add("Zatrzymaj siê w tej chwili! Dalej nie przejdziesz, chyba ¿e zap³acisz mi 3000 sztuk z³ota!");

            npc.playerQuestDialogue.Add("Chyba œnisz! Lepiej zejdŸ mi z drogi!");

            npc.npcQuestDialogue.Add("Powtarzam jeszcze raz! Albo 3000 z³ota albo gorzko tego po¿a³ujesz!");

            npc.playerQuestDialogue.Add("Trzymaj te swoje 3000 i zostaw mnie w spokoju!"); 
            npc.playerQuestDialogue.Add("Nic ode mnie nie dostaniesz z³odzieju!"); 

            npc.npcQuestDialogue.Add("Noo i to mi siê podoba! Proszê bardzo mo¿esz iœæ dalej!");
            npc.npcQuestDialogue.Add("Ajj i widzisz! Mia³em dla ciebie grzeczn¹ propozycjê! Sam tego chcia³eœ!");


        }
    }
    public void AddQuestRewardDialogues(NPC npc)
    {
        if (npc._name == "John")
        {
            ClearDialogueLists(npc);

            npc.playerTalkDialogue.Add("Mo¿esz ju¿ wróciæ bezpiecznie do domu!");
            npc.npcTalkDialogue.Add("Nie masz pojêcia jak bardzo jestem Ci wdziêczny wêdrowcze!");
            npc.playerTalkDialogue.Add("Starczy tej wdziêcznoœci! Dawaj nagrodê!");
            npc.npcTalkDialogue.Add("Oczywiœcie! Myœlê ¿e bêdziesz zadowolony trzymaj!");
            dialogueAdded = true;
        }
        if(npc._name == "Mark")
        {
            ClearDialogueLists(npc);

            npc.playerTalkDialogue.Add("Proszê oto twój chleb!");
            npc.npcTalkDialogue.Add("Ooo brawo! Wiedzia³em ¿e dasz sobie radê.");
            npc.playerTalkDialogue.Add("Co z moj¹ nagrod¹?");
            npc.npcTalkDialogue.Add("Ale¿ proszê ciê bardzo! Trzymaj!");
            dialogueAdded = true;
        }
        if(npc._name == "Martin")
        {
            ClearDialogueLists(npc);

            npc.playerTalkDialogue.Add("Noo g³adko posz³o przyjacielu!");
            npc.npcTalkDialogue.Add("Nie ma silnych na dwóch paladynów!");
            npc.playerTalkDialogue.Add("Mia³eœ racjê bez twojej pomocy nie da³bym rady.");
            npc.npcTalkDialogue.Add("Nie martw siê wojowniku! Nawet mi by³oby cie¿ko walczyæ z tymi potworami w pojedynkê! Nie zawiod³em siê na tobie, trzymaj za to nagrodê!");
            dialogueAdded = true;
        }
        if (npc._name == "Bulit")
        {
            ClearDialogueLists(npc);

            npc.playerTalkDialogue.Add("Wielkie kraby ju¿ wiêcej nie bêd¹ niepokoiæ ludzi! Wszystkie w¹chaj¹ kwiatki od spodu!");
            npc.npcTalkDialogue.Add("Niesamowite! A jednak uda³o ci siê! Brawo!");
            npc.playerTalkDialogue.Add("No udaæ mi siê uda³o, ale by³o to nie lada wyzwanie! To co z moim z³otem?");
            npc.npcTalkDialogue.Add("Oczywiœcie! Ja zawsze dotrzymujê s³owa. Oto twoje 2000 z³ota!");
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
