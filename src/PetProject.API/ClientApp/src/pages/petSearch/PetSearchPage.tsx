import React from 'react';
import { connect } from 'react-redux';
import { groupBy } from 'lodash';
import { Theme, createStyles, WithStyles, withStyles } from '@material-ui/core';
import GridList from '@material-ui/core/GridList';
import GridListTile from '@material-ui/core/GridListTile';
import GridListTileBar from '@material-ui/core/GridListTileBar';
import ListSubheader from '@material-ui/core/ListSubheader';
import IconButton from '@material-ui/core/IconButton';
import InfoIcon from '@material-ui/icons/Info';

import FormLabel from '@material-ui/core/FormLabel';
import FormControl from '@material-ui/core/FormControl';
import FormGroup from '@material-ui/core/FormGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';

import { AppState } from '../../store/appState';
import { Pet } from '../../store/petsList/state';
import { requestPetsListData } from '../../store/petsList/actions';
import { Feature } from '../../store/featuresList/state';
import { requestFeaturesListData } from '../../store/featuresList/actions';

const styles = (theme: Theme) => createStyles({
    root: {
      display: 'flex',
      flexWrap: 'wrap',
      justifyContent: 'space-around',
      overflow: 'hidden',
      backgroundColor: theme.palette.background.paper,
    },
    gridList: {
      width: 500,
      // height: 450,
    },
    icon: {
      color: 'rgba(255, 255, 255, 0.54)',
    },
    formControl: {
      margin: theme.spacing(3),
    },
  });

interface PetSearchProps extends WithStyles<typeof styles> {
    pets: Pet[];
    features: Feature[];
    loadPetsList: () => void;
    loadFeaturesList: () => void;
};

interface PetSearchState {

};

 const PetSearchPageStyled = withStyles(styles)(
class PetSearchPage extends React.Component<PetSearchProps, PetSearchState> {
    constructor(props: PetSearchProps) {
        super(props);
        this.state = {
        };
    }

    componentDidMount() {
        this.props.loadPetsList();
        this.props.loadFeaturesList();
    }

    render() {
        const { pets, classes } = this.props;
        return (
            <div className={classes.root}>
                <FormControl component="fieldset" className={classes.formControl}>
                    <FormLabel component="legend">Assign responsibility</FormLabel>
                    <FormGroup>
                    <FormControlLabel
                        control={<Checkbox checked={false} value="gilad" />}
                        label="Gilad Gray"
                    />
                    <FormControlLabel
                        control={<Checkbox checked={true} value="jason" />}
                        label="Jason Killian"
                    />
                    <FormControlLabel
                        control={<Checkbox checked={false} value="antoine" />}
                        label="Antoine Llorca"
                    />
                    </FormGroup>
                </FormControl>
                <GridList cellHeight={180} className={classes.gridList}>
                    <GridListTile key="Subheader" cols={2} style={{ height: 'auto' }}>
                        <ListSubheader component="div">Pets list:</ListSubheader>
                    </GridListTile>
                    {pets.map(pet => (
                        <GridListTile key={pet.petId}>
                            <img src={pet.images[0].imagePath} alt={pet.name} />
                            <GridListTileBar
                                title={pet.name}
                                subtitle={<span>by: {pet.description}</span>}
                                actionIcon={
                                    <IconButton aria-label={`info about ${pet.name}`} className={classes.icon}>
                                        <InfoIcon />
                                    </IconButton>
                                }
                            />
                        </GridListTile>
                    ))}
                </GridList>
            </div>
        );
    }
}
);

export const PetSearchPageConnected = connect(
    (appState: AppState) => ({
        pets: appState.pets.data,
        features: appState.features.data
    }),
    {
        loadPetsList: requestPetsListData,
        loadFeaturesList: requestFeaturesListData,
    }
)(PetSearchPageStyled);
