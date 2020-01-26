import React, { Component } from 'react';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import { FormControl, FormGroup, FormControlLabel, Checkbox, FormHelperText } from '@material-ui/core';

import { AppState } from '../../../store/appState';
import { TaskType } from '../../../store/taskTypesList/state';
import { requestTaskTypesListData } from '../../../store/taskTypesList/actions';
import { receiveTaskTypesList } from '../../../store/taskTypesFilters/actions';
import { TaskTypeFilter } from '../../../store/taskTypesFilters/state';

interface TaskTypesModalProps {
    open: boolean;
    taskTypes: TaskType[];
    handleCancel: () => void;
    handleSubmit: () => void;
    loadTaskTypesData: () => void;
    setTaskTypesFilters: (taskTypes: TaskTypeFilter[]) => void;
}

interface TaskTypeCheckbox {
    id: string;
    name: string;
    checked: boolean;
}

interface TaskTypesModalState {
    taskTypes: TaskTypeCheckbox[];
}

class TaskTypesModal extends Component<TaskTypesModalProps, TaskTypesModalState> {
    constructor(props: TaskTypesModalProps) {
        super(props);
        this.state = {
            taskTypes: []
        };
    }

    componentDidMount() {
        this.props.loadTaskTypesData();
    }

    componentDidUpdate() {
        if (this.state.taskTypes.length !== this.props.taskTypes.length) {
            this.state = {
                taskTypes: this.props.taskTypes.map(f => {
                    return {
                        id: f.taskTypeId,
                        name: f.name,
                        checked: false,
                    };
                }),
            };
        }
    }

    setInput = (fieldName: string, newValue: string) => {
        this.setState({ ...this.state, [fieldName]: newValue });
    }

    changeTaskTypeCheckbox = (id: string) => {
        const { taskTypes } = this.state;
        const taskType = taskTypes.find(f => f.id === id);
        if (taskType !== undefined) {
            const checked = !taskType.checked;
            taskType.checked = checked;
            this.setState({
                taskTypes: taskTypes
            });
        }
    }

    handleSuccess = () => {
        const taskTypesFilters = this.state.taskTypes.map(tt => {
            return {
                taskTypeId: tt.id,
                name: tt.name,
                checked: tt.checked
            }
        });
        this.props.setTaskTypesFilters(taskTypesFilters);
        this.props.handleSubmit();
    }

    render() {
        const { open, handleCancel, taskTypes } = this.props;
        return (
            <div>
                <Dialog open={open} onClose={handleCancel} aria-labelledby="form-dialog-title">
                    <DialogTitle id="form-dialog-title">Я готова, я готов...</DialogTitle>
                    <DialogContent>
                        <FormControl component="fieldset">
                            <FormGroup>
                                {taskTypes.map(tt => {
                                    const checked = this.state.taskTypes.find(t => tt.taskTypeId == t.id)?.checked;
                                    return (
                                        <>
                                            <FormControlLabel
                                                control={
                                                <Checkbox 
                                                    value="pr-agent" 
                                                    checked={checked}
                                                    onChange={() => this.changeTaskTypeCheckbox(tt.taskTypeId)}
                                                />}
                                                label={tt.name}
                                            />
                                            <FormHelperText style={{ fontSize: '15px' }}>
                                                {tt.description}
                                            </FormHelperText>
                                            <FormHelperText style={{ fontSize: '15px' }}>
                                                Срок на выполнение: {tt.defaultDurationDays} дней.
                                            </FormHelperText>
                                        </>
                                    );
                                })}
                            </FormGroup>
                        </FormControl>
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleCancel} color="secondary">
                            Отмена
                    </Button>
                        <Button onClick={() => this.handleSuccess()} color="primary">
                            Найти подопечного
                  </Button>
                    </DialogActions>
                </Dialog>
            </div>
        );
    }
}

export const TaskTypesModalConnected = connect(
    (appState: AppState) => ({
        taskTypes: appState.taskTypes.data
    }),
    {
        loadTaskTypesData: requestTaskTypesListData,
        setTaskTypesFilters: receiveTaskTypesList
    }
)(TaskTypesModal);
