export interface TaskType {
    taskTypeId: string;
    name: string;
    description: string;
    defaultDurationDays: number
}

export interface TaskTypesListState {
    data: TaskType[];
}