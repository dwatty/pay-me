interface IProps {
    hasSeenNext: boolean;
    hasDiscarded: boolean;
    isYourTurn: boolean;
}

export const TurnBadge = (props : IProps) => {

    const getTurnState = () => {

        if(props.isYourTurn && props.hasDiscarded && props.hasSeenNext) {
            return "Waiting to End Turn";
        }

        if(props.isYourTurn && props.hasSeenNext && !props.hasDiscarded) {
            return "Waiting to Discard";
        }

        if(props.isYourTurn && !props.hasSeenNext && !props.hasDiscarded) {
            return "Waiting to Draw";
        }

        return "Not My Turn";
    }

    return (
        <div className="turnBadge">{ getTurnState() }</div>
    )


}