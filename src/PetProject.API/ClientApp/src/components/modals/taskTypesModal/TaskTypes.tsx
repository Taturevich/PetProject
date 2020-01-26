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

interface TaskTypesModalProps {
    open: boolean;
    taskTypes: TaskType[];
    handleCancel: () => void;
    handleSubmit: () => void;
    loadTaskTypesData: () => void;
}

interface TaskTypesModalState {
    name: string;
    description: string;
}

class TaskTypesModal extends Component<TaskTypesModalProps, TaskTypesModalState> {
    constructor(props: TaskTypesModalProps) {
        super(props);
        this.state = {
            name: '',
            description: '',
        };
    }

    componentDidMount() {
        console.log(111);
        this.props.loadTaskTypesData();
      }

    setInput = (fieldName: string, newValue: string) => {
        this.setState({ ...this.state, [fieldName]: newValue });
    }

    render() {
        const { open, handleSubmit, handleCancel, taskTypes } = this.props;
        const { name, description } = this.state;
        return (
            <div>
                <Dialog open={open} onClose={handleCancel} aria-labelledby="form-dialog-title">
                    <DialogTitle id="form-dialog-title">Я готова, я готов...</DialogTitle>
                    <DialogContent>
                        <FormControl component="fieldset">
                            <FormGroup>
                                {taskTypes.map(tt => {
                                    return (
                                        <>
                                            <FormControlLabel 
                                                control={<Checkbox value="pr-agent" />} 
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
                        <Button onClick={handleSubmit} color="primary">
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
        loadTaskTypesData: requestTaskTypesListData
    }
)(TaskTypesModal);
