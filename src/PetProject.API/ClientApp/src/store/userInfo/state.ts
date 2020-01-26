export interface UserInfo {
    accommodation: number;
    isRented: boolean;
    experience: number;
    additionalOptions: number[];
}

export interface UserInfoListState {
    data: UserInfo[];
}