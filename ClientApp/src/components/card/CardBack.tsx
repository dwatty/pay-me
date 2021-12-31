import './Card.css';

interface IProps {
    onClick?: any;
}

export const CardBack = (props : IProps) => {
    return (
        <div
            className="card back" 
            onClick={ props.onClick ? props.onClick : null }
            key="card-back-placeholder">
            
        </div>
    )
}