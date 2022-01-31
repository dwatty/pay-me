import { useEffect, useState } from "react";
import { DragDropContext, Droppable, Draggable } from "react-beautiful-dnd";
import { Card } from "../../models/card";
import { CardComponent } from "../card/CardComponent";

const reorder = (list: any[], startIndex: number, endIndex: number) => {
    const result = Array.from(list);
    const [removed] = result.splice(startIndex, 1);
    result.splice(endIndex, 0, removed);

    return result;
};

interface IProps {
    hand: Array<Card[]>;
    onCardClick: any;
    onNewGroupClick: any;
}

export const PlayerHand = (props: IProps) => {
    const [state, setState] = useState<Array<Card[]>>([]);

    useEffect(() => {
        setState([...props.hand]);
    }, [props.hand]);
    /**
     * Moves an item from one list to another list.
     */
    const move = (
        source: any,
        destination: any,
        droppableSource: any,
        droppableDestination: any
    ) => {
        const sourceClone = Array.from(source);
        const destClone = Array.from(destination);
        const [removed] = sourceClone.splice(droppableSource.index, 1);

        destClone.splice(droppableDestination.index, 0, removed);

        const result = {} as any;
        result[droppableSource.droppableId] = sourceClone;
        result[droppableDestination.droppableId] = destClone;

        return result;
    };

    const grid = 8;

    const getItemStyle = (isDragging: boolean, draggableStyle: any, index: number) => ({
        userSelect: "none",
        background: isDragging ? "lightgreen" : "transparent",
        position: 'absolute',
        top: '12px',
        left: `${ index * 55 }px`,
        // styles we need to apply on draggables
        ...draggableStyle,
    });

    const getListStyle = (isDraggingOver: boolean) => ({
        background: isDraggingOver ? "#DDD" : "#EEE",
        //padding: grid,
        //width: 250,
    });

    function onDragEnd(result: any) {
        const { source, destination } = result;

        // dropped outside the list
        if (!destination) {
            return;
        }
        const sInd = +source.droppableId;
        const dInd = +destination.droppableId;

        if (sInd === dInd) {
            const items = reorder(state[sInd], source.index, destination.index);
            const newState = [...state];
            newState[sInd] = items;
            setState(newState);
        } else {
            const result = move(state[sInd], state[dInd], source, destination);
            const newState = [...state];
            newState[sInd] = result[sInd];
            newState[dInd] = result[dInd];

            setState(newState.filter((group) => group.length));
        }
    }

    return (
        <div className="my-cards">
            <DragDropContext onDragEnd={onDragEnd}>
            {
                state.map((el, ind) => (
                    <Droppable key={ind} droppableId={`${ind}`} direction="horizontal">
                        {(provided, snapshot) => (
                            <div
                            className="wrapper"
                                ref={provided.innerRef}
                                style={getListStyle(snapshot.isDraggingOver)}
                                {...provided.droppableProps}
                            >
                            {
                                el.map((item, index) => (
                                    <Draggable
                                        key={item.id}
                                        draggableId={item.id}
                                        index={index}
                                        
                                    >
                                    {
                                        (provided, snapshot) => (
                                            <div
                                            className="blahblah"
                                                ref={provided.innerRef}
                                                {...provided.draggableProps}
                                                {...provided.dragHandleProps}
                                                style={getItemStyle(
                                                    snapshot.isDragging,
                                                    provided.draggableProps
                                                        .style,
                                                    index
                                                )}
                                            >                                            
                                                <CardComponent
                                                    key={item.id}
                                                    suite={item.suite}
                                                    value={item.value}
                                                    click={() =>
                                                        props.onCardClick(
                                                            item.suite,
                                                            item.value
                                                        )
                                                    }
                                                />                                                
                                            </div>
                                        )
                                    }
                                    </Draggable>
                                ))}
                                {provided.placeholder}
                            </div>
                        )}
                    </Droppable>
            ))}
            </DragDropContext>
        </div>
    );
};