import { Loading } from "../loading/Loading";

export const GameWaiting = () => {
    return (
        <div className="table-background">
            <div className="waiting-screen">
                <h1>Hmm...</h1>
                <p>
                    Looks like you need some friends... I guess we'll just wait.
                </p>
                <Loading />
            </div>
        </div>
    );
};
