import React, { Component } from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import { Typography, Theme, createStyles, WithStyles, withStyles, Grid } from '@material-ui/core';

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
              <Dialog 
                  open={open} 
                  aria-labelledby="form-dialog-title"
                  fullWidth={true}
                  maxWidth = {'lg'}>
                <DialogTitle id="form-dialog-title">О питомце</DialogTitle>
                <DialogContent>
                  <Grid container spacing={3}>
                    <Grid item xs={3}>
                      <img src="images/barsik.jpg" alt="Барсик" />
                    </Grid>
                    <Grid item xs={4}>
                      <Typography>Барсик</Typography>>
                      <Typography>Пирожок.</Typography>>
                    </Grid>
                    <Grid item xs={5}>                      
                      <Typography>Кот</Typography>>
                      <Typography>Мальчик</Typography>>
                      <Typography>До 3 лет</Typography>>
                      <Typography>Шерсть:</Typography>>
                      <Typography>Короткая</Typography>>
                      <Typography>Темная</Typography>>
                    </Grid>
                  </Grid>
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