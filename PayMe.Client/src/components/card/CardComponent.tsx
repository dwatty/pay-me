import { useMemo } from "react";
import { Suites } from "../../models/enums"
import './Card.css';

import heartSvg from '../../content/heart.svg';
import diamondSvg from '../../content/diamond.svg';
import spadeSvg from '../../content/spade.svg';
import clubSvg from '../../content/club.svg';
import jokerSvg from '../../content/joker.svg';

interface IProps {
    suite: Suites;
    value: number;
    click: any;
}

interface IDisplayProps {
    suite: Suites;
    value: number;
}

export const CardDisplay = (props: IDisplayProps) => {

    const getValue = useMemo(() => {
        switch (props.value)
        {
            case 14:
                return "A";
            case 13:
                return "K";
            case 12:
                return "Q";
            case 11:
                return "J";
            case 0:
                 return 'X';
            default:
                return props.value;
        }
    }, [props.value]);

    const getSuite = useMemo(() => {
        switch(props.suite) {
            case Suites.Clubs:
                return <img alt="Club Suit" src={clubSvg} />;
            case Suites.Hearts:
                return <img alt="Heart Suit" src={heartSvg} />;
            case Suites.Diamonds:
                return <img alt="Diamond Suit" src={diamondSvg} />;
            case Suites.Spades:
                return <img alt="Spade Suit" src={spadeSvg} />;
            case Suites.Jokers:
                return <img alt="Joker Suit" src={jokerSvg} />;
            default:    
            return <img alt="Placeholder" src={''} />;
        }
    }, [props.suite]);

    return (
        <div className="card-display">
            <span className="value">{ getValue }</span> 
            { getSuite }
        </div>
    )

}

export const CardComponent = (props : IProps) => {

    const getCardClass = useMemo(() => {
        switch(props.suite) {
            case Suites.Diamonds:
            case Suites.Hearts:
                return "card red";
            case Suites.Spades:
            case Suites.Clubs:
            default:
                return "card black";
        }
    }, [props.suite]);

    return (
        <div onClick={props.click} className={ getCardClass }>
            <CardDisplay suite={props.suite} value={props.value} />
            <CardDisplay suite={props.suite} value={props.value} />
        </div>
    )
}