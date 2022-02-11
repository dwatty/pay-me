export type ToastMessage = {
    id: number;
    title: string,
    description: string,
    type: string;
};

export type AddToastAction = {
    title: string,
    description: string,
    type: string;
}