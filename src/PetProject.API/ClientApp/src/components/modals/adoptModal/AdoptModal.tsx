import React, { Component } from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import { Typography, Theme, createStyles, WithStyles, withStyles } from '@material-ui/core';

const styles = (theme: Theme) => createStyles({
  root: {
      display: 'flex',
      flexWrap: 'wrap',
      justifyContent: 'space-around',
      overflow: 'hidden',
      backgroundColor: theme.palette.background.paper,
  }
});

interface AdoptModalProps extends WithStyles<typeof styles> {
    open: boolean;
    handleAdopt: () => void;
}

interface AdoptModalState {
    name: string;
    description: string;
    imagePath: string;
    descriptionFeatures: string[];
    ageFeatures: string[];
    furFeatures: string[];
}

export const AdoptModalStyled = withStyles(styles)(
  class AdoptModal extends Component<AdoptModalProps, AdoptModalState> {
    constructor(props: AdoptModalProps){
        super(props);
        this.state = {
            name: "",
            description: "",
            imagePath: "",
            descriptionFeatures: [],
            ageFeatures: [],
            furFeatures: []
        };
    }

    render() {
        const { open, handleAdopt, classes } = this.props;

        return (
              <Dialog open={open} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">О питомце</DialogTitle>
                <DialogContent>

                  <div className={classes.root}>
                    <img src="images/barsik.jpg" alt="Барсик" />
                    <div>
                      <Typography
                          component="span"
                          variant="subtitle1"
                          color="inherit"
                      >
                          Барсик
                      </Typography>
                      <Typography
                          component="span"
                          variant="subtitle1"
                          color="inherit"
                      >
                          Пирожок.
                      </Typography>
                    </div>
                    <div>
                      <Typography
                          component="span"
                          variant="subtitle1"
                          color="inherit"
                      >
                          Кот
                      </Typography>
                      <Typography
                          component="span"
                          variant="subtitle1"
                          color="inherit"
                      >
                          Мальчик
                      </Typography>
                      <Typography
                          component="span"
                          variant="subtitle1"
                          color="inherit"
                      >
                          До 3 лет
                      </Typography>
                      <Typography
                          component="span"
                          variant="subtitle1"
                          color="inherit"
                      >
                          Шерсть:
                      </Typography>
                      <Typography
                          component="span"
                          variant="subtitle1"
                          color="inherit"
                      >
                          Короткая
                      </Typography>
                      <Typography
                          component="span"
                          variant="subtitle1"
                          color="inherit"
                      >
                          Темная
                      </Typography>
                    </div>
                  </div>
                </DialogContent>
                <DialogActions>
                  <Button onClick={handleAdopt} color="primary">
                    Приютить!
                  </Button>
                </DialogActions>
              </Dialog>
          );
    }
  }
);