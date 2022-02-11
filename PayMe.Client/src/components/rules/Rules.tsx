import { NavMenu } from "../navbar/NavMenu";
import { SectionHeader } from "../shared/SectionHeader";
import { ContentWrap } from '../shared/ContentWrap';

export const Rules = () => {

    
    return (
        <>
            <NavMenu />

            <ContentWrap>
                
                <SectionHeader title="What's Pay Me?" />
                
                <div className="row">
                    <div className="col">
                        <p>
                            Pay Me is a card game based around the concept of groups and runs of cards.  A game consists
                            of 12 rounds, each round adding an additional card to the player's hand.  (For the "Threes" round, 
                            a player is dealt 3 cards, for the "Fours" round a player is dealt 4 cards, and so on.).
                        </p>
                        <p>
                            When someone is able to create enough groups and runs such that all the cards in their hand are accounted for, that
                            player wins that round.  After that, each remaining player gets 1 turn to try and minimize their score
                            as much as possible.  The winner of the game is the player with the fewest points at the end of the twelfth
                            round is the winner.
                        </p>
                    </div>
                </div>
                
                <SectionHeader title="How To Play" />

                <div className="row mb-3">
                    <div className="col">                        
                        <p>                            
                            The game starts with  each player being dealt 3 cards each.  One card is then placed face up in the middle 
                            of the table - this is the <strong>discard pile</strong>.  The goal of this round is get three of a kind or 
                            a run of 3 cards.
                        </p>
                        <p>
                            The player's turn begins with a choice - will you pick up the card that's on the top of the discard pile? 
                            Or will you draw an unknown card from the remaining deck?  In either case, the card you select becomes
                            a card in your hand, giving you 4 in total.
                        </p>
                        <p>
                            Now you must try to create a winning hand of three cards (since we're on the "Threes" round) from the 4 cards 
                            you have.  You can create a 3-of-a-kind or a run of 3 in a row (more details on these later.)  Once you decide 
                            which card you'll get rid of, discard that back to the pile.  This will be the new card that the next player
                            can choose to pick up when it's their turn.                            
                        </p>
                        <p>
                            After you've discarded your card, you can end your turn or claim a victory.  If you have a valid combination,
                            click the "Claim Win" button, otherwise click the "End Turn" button to let the next player start.
                        </p>
                    </div>
                </div>

                <SectionHeader title="What's a Group?  What's a run?" />

                <div className="row">
                    <div className="col">
                        <p>
                            Groups and runs are the foundation for constructing winning hands in Pay Me.  Both
                            a group and a run must be at least 3 cards but can be any size larger than that.  A
                            player's hand can contain multiple groups and run as well.  Consider
                        </p>

                        <h3>Groups</h3>
                        <p>
                            A group is a collection of cards that all have the same face value.
                        </p>

                        <ContentWrap>
                            <h4>Valid Examples</h4>
                            <ul>
                                <li>2H, 2D, 2S is a valid three of a kind.</li>
                                <li>KD, KS, KC, KH is a valid group of four of a kind.</li>
                            </ul>

                            <h4>Invalid Examples</h4>
                            <ul>
                                <li>2H, 2D, 3D is invalid because all the faces to not match.</li>
                            </ul>
                        </ContentWrap>

                        <h3>Runs</h3>
                        <p>
                            A run is a collection of at-least 3 cards that all have the same suite and appear
                            in consecutve order.
                        </p>

                        <ContentWrap>
                            <h4>Valid Examples</h4>
                            <ul>
                                <li>2H, 3H, 4H is a run of three hearts and considered valid.</li>
                                <li>10D, JD, QD, KD is a run of four diamonds and considered valud.</li>
                            </ul>

                            <h4>Invalid Examples</h4>
                            <ul>
                                <li>2H, 3D, 4H is invalid because all the suits do not match.</li>
                                <li>9D, 10D, QD, KD is invalid because the cards are not consecutive.</li>
                            </ul>
                        </ContentWrap>

                        <p>
                            As mentioned before, groups and run must be at least 3 cards big, but can be any size 
                            beyond that as long as it fits in your hand.  Consider that you have the follow hand:
                            Our Hand: 2H, 3H, 4H, 5H, 8D, 8H, 8S.
                        </p>
                        <p>
                            You can break this into 2 collections.  You have a run of 4 (2H-3H-4H-5H) and a three
                            of a kind (8D, 8H, 8S).  If we're playing the "Sevens" round, then this would be a winning hand.
                        </p>

                        
                    </div>
                </div>
                

                <SectionHeader title="Wilds" />

                <div className="row">
                    <div className="col">
                        <p>
                            For each round, there is a different <strong>wild card</strong>.  The wild card for each round is the
                            card that represents the current round.  For the "Threes" round, all suits of the 3 are wild.  For the 
                            "Nines" round, all suits of the 9 are wild.  For the "Aces" round, all suits of the Ace are wild.  A wild
                            is unique in that it can be played as any suit and any card.
                        </p>
                        <p>
                            Consider that you're playing the "Fives" round and your hand is the following: 10H, 5D, QH, KH, AH.  In this
                            case, since we're on "Fives", the 5 is wild and can be anything.  Here, we're going to treat our 5D as a JH, 
                            giving us a run of 5 cards and a winning hand.
                        </p>
                    </div>
                </div>

                <SectionHeader title="Scoring" />

                <div className="row">
                    <div className="col">
                        <p>
                            A player's score is calcualted at the end of every round.  
                            The cards that remain in a player's hand after making their
                            runs and groups are counted up based on a few sets of criteria.
                        </p>

                        <ul>
                            <li>Cards 2,3,4,5,6,7,8, and 9 are all worth 5 points</li>
                            <li>Cards 10, Jack, Queen, King are all worth 10 points</li>
                            <li>Cards Ace and Joker are all worth 15 points </li>                            
                        </ul>

                        <p>
                            Additionally, the card that represents the current round (i.e. the wild card)
                            is also worth 15 points for that round.  So if you're playing the 5's round, 
                            then the 5 is worth 15 points if left in your hand.
                        </p>

                        <p>
                            Once the game reaches the Queens round, all point values double for 
                            Queen, King and Ace.
                        </p>

                        <ContentWrap>
                            <h2>Examples</h2>

                            <ul>
                                <li>In your hand you have: 5, 8, 10, Ace.  Your score is (5 + 5 + 10 + 15) = 35.</li>
                                <li>In your hand you have: 3, 10, 7.  Your score is (15 + 10 + 5) = 30.</li>
                                <li>In your hand you have: 10, Queen, Ace.  Your score is (10 + 10 + 10) = 30.</li>
                            </ul>
                        </ContentWrap>
                        
                    </div>
                </div>

            </ContentWrap>
        </>
    );
};
