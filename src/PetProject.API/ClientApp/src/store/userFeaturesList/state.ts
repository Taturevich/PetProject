export interface UserFeature {
    userFeatureId: string;
    category: string;
    characteristic: string;
}

export interface UserFeaturesListState {
    data: UserFeature[];
}