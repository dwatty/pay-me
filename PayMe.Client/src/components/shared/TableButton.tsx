import styled from 'styled-components';

type TableButtonProps = {
    top: string;
    left: string;
    right: string;
}

/**
 * A button component for display on the "felt" table
 */
export const TableButton = styled
    .button
    .attrs<TableButtonProps, TableButtonProps>((props: TableButtonProps) => {
        return {
            top: props.top,
            left: props.left,
            right: props.right,
            className: 'btn'
        }
})<TableButtonProps>`
    display: block;
    position: absolute;
    top: ${ props => props.top ||  "1.5rem" };
    left: ${ props => props.left ||  "auto" };
    right: ${ props => props.right ||  "auto" };
    color: white;
    border-color: white;

    &:hover {
        color: #eee;
    }
`;

/**
 * A link component for display on the "felt" table
 */
export const TableLink = styled
    .a
    .attrs<TableButtonProps, TableButtonProps>((props: TableButtonProps) => {
        return {
            top: props.top,
            left: props.left,
            right: props.right,
            className: 'btn'
        }
})<TableButtonProps>`
    display: block;
    position: absolute;
    top: ${ props => props.top ||  "1.5rem" };
    left: ${ props => props.left ||  "auto" };
    right: ${ props => props.right ||  "auto" };
    color: white;
    border-color: white;

    &:hover {
        color: #eee;
    }
`;