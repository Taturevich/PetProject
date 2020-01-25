export interface Image {
    imagePath: string;
}

export interface Pet {
    petId: string;
    name: string;
    description: string;
    images: Image[];
}

export interface PetsListState {
    data: Pet[];
}