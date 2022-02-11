interface IProps {
    title: string
}

export const SectionHeader = (props: IProps) => {
    return (
        <div className="row mb-3">
            <div className="col">
                <h1 className="mb-0">{ props.title }</h1>
                <hr/>
            </div>
        </div>
    )
}