export interface Tour {
    id?: number;
    name: string;
    description: string;

    from: string;
    to: string;

    transportType: string;
    distance: number;
    estimatedTime: string;

    popularity?: string;
    childFriendliness?: string;

    imagePath?: string;

    stops?: string[];

    routeGeometry?: string;
}
