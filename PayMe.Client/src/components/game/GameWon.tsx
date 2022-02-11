interface IProps {
    isVisible: boolean;
}

export const GameWon = (props: IProps) => {

    return props.isVisible
        ? <div className="game-won">
            <h1>You Won!</h1>
            <p>Bask in the glory of your win while the other players finish up.</p>
        </div>
        : null    
}