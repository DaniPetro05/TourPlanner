export interface Tour {
    id: number;
    name: string;
    description: string;
    imagePath?: string;

    //lat?: number;
    //lng?: number;

    from: string;
    to: string;

    stops?: string[];
}
