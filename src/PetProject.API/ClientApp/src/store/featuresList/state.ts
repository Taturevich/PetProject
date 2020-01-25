export interface Feature {
    petFeatureId: string;
    category: string;
    characteristic: string;
}

export interface FeaturesListState {
    data: Feature[];
}