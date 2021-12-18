import { useMemo } from "react";
import { Suites } from "../../models/enums"
import './Card.css';

import heartSvg from '../../content/heart.svg';
import diamondSvg from '../../content/diamond.svg';
import spadeSvg from '../../content/spade.svg';
import clubSvg from '../../content/club.svg';
import jokerSvg from '../../content/joker.svg';

interface IProps {
    key: number;
    suite: Suites;
    value: number;
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
                return <img src={clubSvg} />;
            case Suites.Hearts:
                return <img src={heartSvg} />;
            case Suites.Diamonds:
                return <img src={diamondSvg} />;
            case Suites.Spades:
                return <img src={spadeSvg} />;
            case Suites.Jokers:
                return <img src={jokerSvg} />;
            default:    
            return <img src={''} />;
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

    

    return (
        <div className="card" key={`card-${props.key}`}>
            <CardDisplay suite={props.suite} value={props.value} />
            <CardDisplay suite={props.suite} value={props.value} />
        </div>
    )
}