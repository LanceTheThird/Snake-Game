using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using WpfTestApp.Interfaces;
using WpfTestApp.ViewModels;
using WpfTestApp.Model;
using WpfTestApp.Model.Stepping;

namespace WpfTestApp.ServiceClasses
{
    internal class LevelGenerator : IWallsGenerator
    {
        private readonly int _height, _width, _wallCount, _wallMass;
        private readonly int _step = Constants.Step;
        private readonly ObservableCollection<Block> _walls = new ObservableCollection<Block>();

        private readonly Random _random = Constants.Random;

        public LevelGenerator(int height, int width,  int percentage)
        {
            _height = height;
            _width = width;
            _wallCount = (int) ((height * width * percentage * 0.01)/Math.Pow(_step, 2));
            _wallMass = (int)(1 / (percentage * 0.005));
            if (_wallMass == 0)
                _wallMass = 1;
        }

        public ObservableCollection<Block> Generate( int currentLevel)
        {
            var block = new Block(Constants.Wall, 1 * _step, 1 * _step, 0, ChainType.Wall, 3, 0.3);
            _walls.Add(block);

            for (var i = 1; i < _wallCount; i++)
            {               
                        do
                        { 
                          block = RandomBlock();
                        } while (NewBlockIsBad(block, _walls, i));

                _walls.Add(block);
            }
            return _walls;
        }

        private Block RandomBlock()
        {
            var left = _random.Next(0, _width / _step);
            var top = _random.Next(0, _height / _step);

            return new Block(Constants.Wall, left * _step, top * _step, 0, ChainType.Wall, 3, 0.3);
        }


        private bool NewBlockIsBad(Block block, ObservableCollection<Block> blocks, int i)
        {
            var inRangeBlocks = new List<Block>();
            foreach (var item in blocks)
            {
                if (KeepDistance(block, item) == false)
                    return true;

                if (IsInRange(item, block, 2))
                {
                    inRangeBlocks.Add(item);
                }

            }

            if ((block.Top == Constants.StartTop) && (block.Left >= Constants.StartLeft - Constants.AccelerationBuffer)
                                                  && (block.Left <= Constants.StartLeft + Constants.SnakeStartSize * _step))
                return true;

            if (i % _wallMass == 0)
                return false;

            if (AreManyNeighbours(block, 1, 1))
                return true;

            if (inRangeBlocks.Count >= 4)
            {
                if (!CanGoOut(block, inRangeBlocks))
                    return true;
            }

            return false;
        }

        private bool IsInRange(Block current, Block newOne, int range)
        {
            if ((Math.Abs(current.Left - newOne.Left) <= range * _step) &&
                (Math.Abs(current.Top - newOne.Top) <= range * _step))
                return true;
            return false;
        }

        private bool CanGoOut(Block block, List<Block> blocks)
        {
            Block neighbour = new Block();
            CoordinateStepping stepping = new CoordinateStepping();
            CoordinateStep directionNeigh = new CoordinateStep(0,0);
            foreach (var item in blocks)
            {
                if (AreNeighbours(item, block))
                {
                    neighbour = item;
                    break;
                }
            }

            for (int i = 0; i < stepping.Stepping.Length; i++)
            {
                if (((block.Left - neighbour.Left) == stepping.Stepping[i].Left) &&
                    ((block.Top - neighbour.Top) == stepping.Stepping[i].Top))
                {
                    directionNeigh = stepping.Stepping[i];
                    break;
                }
            }

            var dirTop = new CoordinateStep(2*directionNeigh.Top, 2*directionNeigh.Left);
            var dirTopDown = new CoordinateStep(-2 * directionNeigh.Top, -2 * directionNeigh.Left);

            foreach (Block item in blocks)
            {
                if ((item.Left == block.Left + dirTop.Left) && (item.Top == block.Top + dirTop.Top))
                    return false;
                if ((item.Left == block.Left + dirTopDown.Left) && (item.Top == block.Top + dirTopDown.Top))
                    return false;
            }

            return true;
        }

        private bool AreManyNeighbours(Block block, int min, int max)
        {
            var neighbours = 0;
            Block father = new Block();
            List<Block> crossNeighboursList = new List<Block>();
            foreach (var item in _walls)
            {
                if (AreNeighbours(block, item))
                {
                    neighbours++;
                    father = item;
                }

                if (AreCrossNeighbours(block, item))
                {
                    crossNeighboursList.Add(item);
                }

            }

            if ((neighbours > max) || (neighbours < min))
            {              
                    return true;
            }

            foreach (var cross in crossNeighboursList)
            {
                if (!AreNeighbours(father, cross))
                    return true;
            }

                return false;
        }

        private bool KeepDistance(Block block, Block element)
        {
            if ((Math.Abs(element.Left - block.Left) < _step) && (Math.Abs(element.Top - block.Top) < _step))
                return false;

            return true;
        }

        private bool AreNeighbours(Block newBlock, Block wallBlock)
        {
            if (((wallBlock.Left == newBlock.Left) && (Math.Abs(wallBlock.Top - newBlock.Top) == _step)) ||
                ((wallBlock.Top == newBlock.Top) && (Math.Abs(wallBlock.Left - newBlock.Left) == _step)))
                return true;
            return false;
        }

        private bool AreCrossNeighbours(Block newBlock, Block wallBlock)
        {
            if ((Math.Abs(wallBlock.Left - newBlock.Left) == _step) &&
                (Math.Abs(wallBlock.Top - newBlock.Top) == _step))
                return true;
            return false;
        }


    }
}
