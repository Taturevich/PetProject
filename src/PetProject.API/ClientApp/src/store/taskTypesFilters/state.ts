export interface TaskTypeFilter {
    taskTypeId: string;
    name: string;
    checked: boolean;
}

export interface TaskTypesFiltersListState {
    data: TaskTypeFilter[];
}